using System;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Taledynamic.Core.Exceptions;
using Taledynamic.Core.Interfaces;
using Taledynamic.DAL.Entities;
using Taledynamic.DAL.Models.DTOs;
using Taledynamic.DAL.Models.Requests.TableRequests;
using Taledynamic.DAL.Models.Responses.TableResponses;

namespace Taledynamic.Core.Services
{
    public class TableService: BaseService<Table>, ITableService 
    {
        private TaledynamicContext _context { get; }
        public TableService(TaledynamicContext context) : base(context)
        {
            _context = context;
        }

        public async Task<CreateTableResponse> CreateTableAsync(CreateTableRequest request)
        {
            Log.Information($"[{nameof(TableService)}]: Method '{MethodBase.GetCurrentMethod()?.Name}' started.");
            
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
            
            Log.Information($"[{nameof(TableService)}]: Method '{MethodBase.GetCurrentMethod()?.Name}' ended.");

            return new CreateTableResponse
            {
                StatusCode = (HttpStatusCode) 200,
                Table = new TableDto(table),
                Message = "Success."
            };
        }

        public async Task<GetTablesByWorkspaceResponse> GetTablesByWorkspaceAsync(GetTablesByWorkspaceRequest request)
        {
            Log.Information($"[{nameof(TableService)}]: Method '{MethodBase.GetCurrentMethod()?.Name}' started.");
            
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
            
            Log.Information($"[{nameof(TableService)}]: Method '{MethodBase.GetCurrentMethod()?.Name}' ended.");
            
            return new GetTablesByWorkspaceResponse()
            {
                StatusCode = (HttpStatusCode) 200,
                Message = "Success.",
                Tables = tables
            };
        }

        public async Task<GetTableResponse> GetTableAsync(GetTableRequest request)
        {
            Log.Information($"[{nameof(TableService)}]: Method '{MethodBase.GetCurrentMethod()?.Name}' started.");
            
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
            
            Log.Information($"[{nameof(TableService)}]: Method '{MethodBase.GetCurrentMethod()?.Name}' ended.");
            
            return new GetTableResponse()
            {
                StatusCode = (HttpStatusCode) 200,
                Message = "Success.",
                Table = tableDto
            };
        }

        public async Task<DeleteTableResponse> DeleteTableAsync(DeleteTableRequest request)
        {
            Log.Information($"[{nameof(TableService)}]: Method '{MethodBase.GetCurrentMethod()?.Name}' started.");
            
            var validator = request.IsValid();
            if (!validator.Status)
            {
                throw new BadRequestException(validator.Message);
            }

            await DeleteAsync(request.Id);
            
            Log.Information($"[{nameof(TableService)}]: Method '{MethodBase.GetCurrentMethod()?.Name}' ended.");
            
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
            
            Log.Information($"[{nameof(TableService)}]: Method '{MethodBase.GetCurrentMethod()?.Name}' started.");
            
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
            
            Log.Information($"[{nameof(TableService)}]: Method '{MethodBase.GetCurrentMethod()?.Name}' ended.");
            
            return new UpdateTableResponse()
            {
                StatusCode = (HttpStatusCode) 200,
                Table = new TableDto(table),
                Message = "Success.",
            };
        }
    }
}