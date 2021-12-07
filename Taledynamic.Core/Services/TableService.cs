using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
using Taledynamic.Core.Exceptions;
using Taledynamic.Core.Interfaces;
using Taledynamic.DAL.Entities;
using Taledynamic.DAL.Models.DTOs;
using Taledynamic.DAL.Models.Internal;
using Taledynamic.DAL.Models.Requests.TableRequests;
using Taledynamic.DAL.Models.Responses.TableResponses;
using Taledynamic.DAL.MongoModels;

namespace Taledynamic.Core.Services
{
    public partial class TableService: BaseService<Table>, ITableService 
    {
        private TaledynamicContext _context { get; }
        private IMongoCollection<JsonModel> _documents { get; }
        public TableService(TaledynamicContext context, IMongoDbSettings settings) : base(context)
        {
            _context = context;
            
            var client = new MongoClient(settings.ConnectionString);
            
            // TODO: better approach to handle it with ioc containers
            var database = client.GetDatabase(settings.DatabaseName);
            _documents = database.GetCollection<JsonModel>(settings.JsonCollectionName);
        }

        public async Task<CreateTableResponse> CreateTableAsync(CreateTableRequest request)
        {
            var validator = request.IsValid();
            if (!validator.Status)
            {
                throw new BadRequestException(validator.Message);
            }

            var table = new Table
            {
                IsActive = true,
                Name = request.Name,
                Created = DateTime.Now,
                Modified = DateTime.Now,
                WorkspaceId = request.WorkspaceId
            };

            await this.CreateAsync(table);

            return new CreateTableResponse
            {
                StatusCode = (HttpStatusCode) 200,
                Table = new TableDto(table),
                Message = "Success."
            };
        }

        public async Task<GetTablesByWorkspaceResponse> GetTablesByWorkspaceAsync(GetTablesByWorkspaceRequest request)
        {
            var validator = request.IsValid();
            if (!validator.Status)
            {
                throw new BadRequestException(validator.Message);
            }

            var tables = await _context
                .Tables
                .AsNoTracking()
                .Where(t => t.IsActive && t.WorkspaceId == request.WorkspaceId)
                .Select(t => new TableDto
                {
                    Id = t.Id,
                    Name = t.Name
                })
                .ToListAsync();
            
            return new GetTablesByWorkspaceResponse()
            {
                StatusCode = (HttpStatusCode) 200,
                Message = "Success.",
                Tables = tables
            };
        }

        public async Task<GetTableResponse> GetTableAsync(GetTableRequest request)
        {
            var validator = request.IsValid();
            if (!validator.Status)
            {
                throw new BadRequestException(validator.Message);
            }

            var table = await _context
                .Tables
                .AsNoTracking()
                .FirstOrDefaultAsync(w => w.IsActive && w.Id == request.Id && w.WorkspaceId == request.WorkspaceId);

            if (table == null)
            {
                throw new NotFoundException("Table with requested ids is not found");
            }

            var tableDto = new TableDto(table);
            
            return new GetTableResponse()
            {
                StatusCode = (HttpStatusCode) 200,
                Message = "Success.",
                Table = tableDto
            };
        }

        public async Task<DeleteTableResponse> DeleteTableAsync(DeleteTableRequest request)
        {
            var validator = request.IsValid();
            if (!validator.Status)
            {
                throw new BadRequestException(validator.Message);
            }

            await DeleteAsync(request.Id);
            
            return new DeleteTableResponse()
            {
                StatusCode = (HttpStatusCode) 200,
                Message = "Success.",
            };
        }

        public async Task<UpdateTableResponse> UpdateTableAsync(UpdateTableRequest request)
        {
            // не уверен что изменяемость нужна в этой таблице, сделаю через дефолтный update
            // нет, мне просто лень писать через soft update без очереди, иначе вообще с ума сойти можно с
            // обновлением строк
            var validator = request.IsValid();
            if (!validator.Status)
            {
                throw new BadRequestException(validator.Message);
            }
            
            var table = await _context
                .Tables
                .Include(t => t.Workspace)
                .FirstOrDefaultAsync(t => t.IsActive && t.Id == request.Id);

            if (table == null)
            {
                throw new NotFoundException("Table is not found");
            }
            
            table.Modified = DateTime.Now;
            table.Name = request.Name ?? table.Name;
            await UpdateAsync(table);
            
            return new UpdateTableResponse()
            {
                StatusCode = (HttpStatusCode) 200,
                Table = new TableDto(table),
                Message = "Success.",
            };
        }
    }
}