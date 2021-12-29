using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using Newtonsoft.Json;
using Taledynamic.Core.Exceptions;
using Taledynamic.Core.Helpers;
using Taledynamic.Core.Interfaces;
using Taledynamic.DAL.Entities;
using Taledynamic.DAL.Models.DTOs;
using Taledynamic.DAL.Models.Internal;
using Taledynamic.DAL.Models.Requests.TableRequests;
using Taledynamic.DAL.Models.Requests.TelegramRequests;
using Taledynamic.DAL.Models.Responses;
using Taledynamic.DAL.Models.Responses.UserResponses;
using Taledynamic.DAL.MongoModels;
using JsonSerializer = System.Text.Json.JsonSerializer;
using Table = Taledynamic.DAL.Entities.Table;

namespace Taledynamic.Core.Services
{
    public partial class TelegramService : BaseService<TelegramUser>, ITelegramService
    {
        private TaledynamicContext _context { get; }
        private IMongoCollection<TelegramJson> _documents { get; }
        private UserHelper _userHelper { get; set; }
        private IMongoDbSettings _settings { get; set; }

        public TelegramService(TaledynamicContext context, IMongoDbSettings settings,
            IOptions<AppSettings> appSettings) : base(context)
        {
            _context = context;
            _settings = settings;
            var client = new MongoClient(settings.ConnectionString);

            var database = client.GetDatabase(settings.DatabaseName);
            _documents = database.GetCollection<TelegramJson>(settings.TelegramCollectionName);

            _userHelper = new UserHelper(appSettings);
        }

        public async Task<GenericCreateResponse<TelegramUserDto>> Authorize(TelegramAuthorizeRequest request, User user)
        {
            if (user == null)
            {
                throw new UnauthorizedException("User is not authorized.");
            }

            var telegramUserCheck =
                await _context.TelegramUsers.FirstOrDefaultAsync(
                    t => t.TgUserId == request.TelegramUserId && t.IsActive);
            if (telegramUserCheck != null)
            {
                throw new BadRequestException("User with telegram user id already exist");
            }

            var telegramUser = new TelegramUser
            {
                IsActive = true,
                TgUserId = request.TelegramUserId,
                TgApiKey = request.TelegramApiKey,
                User = user,
            };

            await this.CreateAsync(telegramUser);
            return new GenericCreateResponse<TelegramUserDto>()
            {
                StatusCode = (HttpStatusCode) 200,
                Item = new TelegramUserDto
                {
                    Id = telegramUser.Id,
                    TgUserId = telegramUser.TgUserId,
                    TgApiKey = telegramUser.TgApiKey
                },
                Message = "Success."
            };
        }

        public async Task<GenericGetResponse<TelegramUserDto>> Get(TelegramGetRequest request, User user)
        {
            if (user == null)
            {
                throw new UnauthorizedException("User is not authorized.");
            }

            var telegramUserCheck =
                await _context.TelegramUsers.FirstOrDefaultAsync(t => t.UserId == user.Id && t.IsActive);
            if (telegramUserCheck == null)
            {
                throw new NotFoundException($"telegram user is not found");
            }

            return new GenericGetResponse<TelegramUserDto>
            {
                StatusCode = (HttpStatusCode) 200,
                Message = "Success",
                Item = new TelegramUserDto
                {
                    Id = telegramUserCheck.Id,
                    TgUserId = telegramUserCheck.TgUserId,
                    TgApiKey = telegramUserCheck.TgApiKey,
                }
            };
        }

        public async Task<GenericDeleteResponse<TelegramUserDto>> Revoke(TelegramRevokeRequest request)
        {
            await this.DeleteAsync(request.Id);

            return new GenericDeleteResponse<TelegramUserDto>
            {
                StatusCode = (HttpStatusCode) 200,
                Message = "Success",
            };
        }

        public async Task SyncTable(CreateTelegramDataRequest request, User user)
        {
            //try to find table with existing mongodb table
            var workspace = await _context.Workspaces
                .FirstOrDefaultAsync(workspace =>
                    workspace.User.Equals(user) && workspace.Name == "TelegramWorkspace" && workspace.IsActive);

            if (workspace == null)
            {
                workspace = new Workspace
                {
                    IsActive = true,
                    Name = "TelegramWorkspace",
                    Created = DateTime.Now,
                    Modified = DateTime.Now,
                    User = user,
                    UserId = user.Id
                };
                await _context.Set<Workspace>().AddAsync(workspace);
                await _context.SaveChangesAsync();
            }

            var table =
                await _context.Tables.AsNoTracking().FirstOrDefaultAsync(q =>
                    q.IsTelegramTable == true && q.MongoDbUId != null && q.Workspace == workspace && q.IsActive);


            if (table == null)
            {
                table = new Table
                {
                    IsActive = true,
                    Name = "TelegramTable",
                    Created = DateTime.Now,
                    Modified = DateTime.Now,
                    MongoDbUId = null,
                    IsTelegramTable = true,
                    Workspace = workspace
                };
                await _context.Set<Table>().AddAsync(table);
                await _context.SaveChangesAsync();
            }

            var client = new MongoClient(_settings.ConnectionString);
            var database = client.GetDatabase(_settings.DatabaseName);
            var tables = database.GetCollection<JsonModel>(_settings.JsonCollectionName);

            if (string.IsNullOrEmpty(table.MongoDbUId))
            {
                var jsonTable = new JsonTableDto
                {
                    Rows = new List<JsonTableRow>()
                    {
                    },
                    Headers = new List<JsonTableHeader>()
                    {
                        new JsonTableHeader(name: "Dates", JsonTableType.Date),
                        new JsonTableHeader(name: "Messages", JsonTableType.Text),
                        new JsonTableHeader(name: "Pictures", JsonTableType.Image),
                    },
                    Immutable = true
                };

                var doc = new JsonModel
                {
                    Id = null,
                    Document = BsonDocument.Parse(JsonConvert.SerializeObject(jsonTable)),
                    Immutable = true,
                };

                await tables.InsertOneAsync(doc);
                table.MongoDbUId = doc.Id;
                _context.Set<Table>().Update(table);
                await _context.SaveChangesAsync();
            }

            var result =
                await tables.FindAsync(document => document.Id == table.MongoDbUId);

            JsonModel jsonModel = result.FirstOrDefault();
            var requestString = JsonSerializer.Serialize(request.JsonContent);
            var message = JsonConvert.DeserializeObject<TelegramUpdateMessage>(requestString);

            DateTime.TryParse(message?.DateTime, out DateTime dateTimeDouble);
           /// Double.TryParse(message?.DateTime, out double dateTimeDouble);
            //var res = UnixTimeStampToDateTime(dateTimeDouble);
            
            var bson = BsonTypeMapper.MapToDotNetValue(jsonModel.Document);
            var serializedDto = JsonSerializer.Serialize(bson);
            var convertedTable = JsonConvert.DeserializeObject<JsonTableDto>(serializedDto);
            convertedTable?.AddRow(dateTimeDouble, message?.Text, message?.Picture);
            var newDoc = new JsonModel
            {
                Id = jsonModel.Id,
                Document = BsonDocument.Parse(JsonConvert.SerializeObject(convertedTable)),
                Immutable = true
            };
            await tables.ReplaceOneAsync(doc => doc.Id == jsonModel.Id, newDoc);
        }

        public async Task<AuthenticateResponse> TgAuthorize(TelegramAuthorizeRequest request)
        {
            var telegramUserCheck =
                await _context.TelegramUsers
                    .Include(t => t.User)
                    .Include(u => u.User.RefreshTokens)
                    .FirstOrDefaultAsync(t => t.TgUserId == request.TelegramUserId && t.IsActive);

            if (telegramUserCheck == null)
            {
                throw new NotFoundException($"telegram user is not found");
            }

            var jwtToken = _userHelper.GenerateJwtToken(telegramUserCheck.User);

            return new AuthenticateResponse
            {
                StatusCode = (HttpStatusCode) 200,
                Message = null,
                JwtToken = jwtToken,
            };
        }

        public static DateTime UnixTimeStampToDateTime(double unixTimeStamp)
        {
            DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dateTime = dateTime.AddSeconds(unixTimeStamp).ToLocalTime();
            return dateTime;
        }
    }

    public class TelegramUpdateMessage
    {
        public string Picture { get; set; }
        public string DateTime { get; set; }

        public string Text { get; set; }
    }

    public class JsonTableDto
    {
        [BsonElement("rows")] 
        [JsonProperty("rows")]

        public List<JsonTableRow> Rows { get; set; }
        [BsonElement("headers")] 
        [JsonProperty("headers")] 

        public List<JsonTableHeader> Headers { get; set; }
        [BsonElement("immutable")] 
        [JsonProperty("immutable")] 

        public bool Immutable { get; set; }

        public void AddRow(DateTime datetime, string text, string picture)
        {
           
            var row = new JsonTableRow(new List<JsonTableCell>()
            {
                new JsonTableCell(datetime.ToString("s", System.Globalization.CultureInfo.InvariantCulture), JsonTableType.Date),
                new JsonTableCell(text, JsonTableType.Text),
                new JsonTableCell(picture, JsonTableType.Image),
            });
            Rows.Add(row);
        }
    }

    public class JsonTableRow
    {
        public JsonTableRow(List<JsonTableCell> cells)
        {
            Cells = cells;
        }

        [BsonElement("cells")] 
        [JsonProperty("cells")]
        public List<JsonTableCell> Cells { get; set; }
    }

    public class JsonTableCell
    {
        public JsonTableCell(string data, JsonTableType type)
        {
            Data = data;
            Type = type;
        }

        [BsonElement("data")]
        [JsonProperty("data")]
        public string Data { get; set; }
        [BsonElement("type")]
        [JsonProperty("type")]
        public JsonTableType Type { get; set; }
    }

    public class JsonTableHeader
    {
        public JsonTableHeader(string name, JsonTableType type)
        {
            Name = name;
            Type = type;
        }

        [BsonElement("name")] 
        [JsonProperty("name")]
        public string Name { get; set; }
        [BsonElement("type")]
        [JsonProperty("type")]
        public JsonTableType Type { get; set; }
    }

    public enum JsonTableType
    {
        Text,
        Number,
        Date,
        Image,
        File
    }
}