using System;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using MongoDB.Bson;
using MongoDB.Driver;
using Newtonsoft.Json;
using Taledynamic.Core.Exceptions;
using Taledynamic.Core.Interfaces;
using Taledynamic.DAL.Entities;
using Taledynamic.DAL.Models.DTOs;
using Taledynamic.DAL.Models.Internal;
using Taledynamic.DAL.Models.Requests.TableRequests;
using Taledynamic.DAL.Models.Requests.TelegramRequests;
using Taledynamic.DAL.Models.Responses;
using Taledynamic.DAL.MongoModels;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace Taledynamic.Core.Services
{
    public partial class TelegramService : BaseService<TelegramUser>, ITelegramService
    {
        private TaledynamicContext _context { get; }
        private IMongoCollection<TelegramJson> _documents { get; }

        public TelegramService(TaledynamicContext context, IMongoDbSettings settings) : base(context)
        {
            _context = context;

            var client = new MongoClient(settings.ConnectionString);

            var database = client.GetDatabase(settings.DatabaseName);
            _documents = database.GetCollection<TelegramJson>(settings.TelegramCollectionName);
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
    }
}