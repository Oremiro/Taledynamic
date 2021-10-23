using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Taledynamic.Core.Entities;
using Taledynamic.Core.Exceptions;
using Taledynamic.Core.Interfaces;
using Taledynamic.Core.Models.DTOs;
using Taledynamic.Core.Models.Requests.TableRequests;
using Taledynamic.Core.Models.Responses.TableResponses;
using Taledynamic.Core.Models.Responses.WorkspaceResponses;

namespace Taledynamic.Core.Services
{
    public class TableService: BaseService<Table>, ITableService 
    {
        private TaledynamicContext _context { get; }
        public TableService(TaledynamicContext context) : base(context)
        {
            _context = context;
        }

        public Task<CreateTableResponse> CreateTableAsync(CreateTableRequest request)
        {
            throw new System.NotImplementedException();
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

            var tableDto = new TableDto
            {
                Id = table.Id,
                Name = table.Name
            };
            return new GetTableResponse()
            {
                StatusCode = (HttpStatusCode) 200,
                Message = "Success.",
                Table = tableDto
            };
        }

        public Task<DeleteTableResponse> DeleteTableAsync(DeleteTableRequest request)
        {
            throw new System.NotImplementedException();
        }

        public Task<UpdateTableResponse> UpdateTableAsync(UpdateTableRequest request)
        {
            throw new System.NotImplementedException();
        }
    }
}