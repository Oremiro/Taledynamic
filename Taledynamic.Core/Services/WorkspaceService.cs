using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Taledynamic.Core.Entities;
using Taledynamic.Core.Exceptions;
using Taledynamic.Core.Interfaces;
using Taledynamic.Core.Models.Internal;
using Taledynamic.Core.Models.Requests.WorkspaceRequests;
using Taledynamic.Core.Models.Responses.WorkspaceResponses;

namespace Taledynamic.Core.Services
{
    public class WorkspaceService : BaseService<Workspace>, IWorkspaceService
    {
        private TaledynamicContext _context { get; set; }

        public WorkspaceService(TaledynamicContext context) : base(context)
        {
            _context = context;
        }

        public async Task<GetWorkspacesByUserResponse> GetFilteredByUserIdAsync(GetWorkspacesByUserRequest request,
            User user)
        {
            if (user == null)
            {
                throw new UnauthorizedException("User is not authorized.");
            }

            var validator = request.IsValid();
            if (!validator.Status)
            {
                throw new BadRequestException(validator.Message);
            }

            var workspaces = await _context
                .Workspaces
                .AsNoTracking()
                .Where(w => w.IsActive && w.User.Equals(user))
                .ToListAsync();

            var response = new GetWorkspacesByUserResponse
            {
                StatusCode = (HttpStatusCode) 200,
                Message = "Success.",
                Workspaces = workspaces
            };

            return response;
        }

        public async Task<GetWorkspaceByIdResponse> GetUserWorkspaceByIdAsync(GetWorkspaceByIdRequest request, User user)
        {
            if (user == null)
            {
                throw new UnauthorizedException("User is not authorized.");
            }

            var validator = request.IsValid();
            if (!validator.Status)
            {
                throw new BadRequestException(validator.Message);
            }

            var workspace = await _context
                .Workspaces
                .AsNoTracking()
                .FirstOrDefaultAsync(w => w.IsActive && w.Id == request.Id && w.User.Equals(user));
            
            return new GetWorkspaceByIdResponse
            {
                StatusCode = (HttpStatusCode) 200,
                Message = "Success.",
                Workspace = workspace
            };
        }

        public async Task<CreateWorkspaceResponse> CreateWorkspaceAsync(CreateWorkspaceRequest request, User user)
        {
            if (user == null)
            {
                throw new UnauthorizedException("User is not authorized.");
            }

            var validator = request.IsValid();
            if (!validator.Status)
            {
                throw new BadRequestException(validator.Message);
            }

            var workspace = new Workspace
            {
                IsActive = true,
                Name = request.Name,
                Created = DateTime.Now,
                Modified = DateTime.Now,
                User = user
            };

            await this.CreateAsync(workspace);

            return new CreateWorkspaceResponse
            {
                StatusCode = (HttpStatusCode) 200,
                Message = "Success."
            };
        }

        public async Task<UpdateWorkspaceResponse> UpdateWorkspaceAsync(UpdateWorkspaceRequest request)
        {

            var validator = request.IsValid();
            if (!validator.Status)
            {
                throw new BadRequestException(validator.Message);
            }

            await using var transation = await _context.Database.BeginTransactionAsync();
            
            var oldWorkspace = await _context
                .Workspaces
                .Include(w => w.User)
                .FirstOrDefaultAsync(w => w.IsActive && w.Id == request.Id);

            if (oldWorkspace == null)
            {
                throw new NotFoundException("Workspace is not found");
            }
            
            
            var newWorkspace = new Workspace
            {
                IsActive = true,
                Name = oldWorkspace.Name,
                Created = oldWorkspace.Created,
                Modified = DateTime.Now,
                User = oldWorkspace.User
            };
            
            await DeleteAsync(oldWorkspace.Id);

            newWorkspace.Name = request.Name ?? newWorkspace.Name;
            await this.CreateAsync(newWorkspace);
            await UpdateTablesForWorkspace(newWorkspace);
            await transation.CommitAsync();
            
            var response = new UpdateWorkspaceResponse()
            {
                StatusCode = (HttpStatusCode) 200,
                Message = "Success."
            };

            return response;
        }
        
        private async Task UpdateTablesForWorkspace(Workspace workspace)
        {
            if (workspace == null)
            {
                throw new BadRequestException("Workspace is not set");
            }
            
            var tables = _context.Tables.Where(w => w.Workspace.Equals(workspace));

            foreach (var table in tables)
            {
                table.Workspace = workspace;
                _context.Update(table);
            }

            await _context.SaveChangesAsync();
        }

        public async Task<DeleteWorkspaceResponse> DeleteWorkspaceAsync(DeleteWorkspaceRequest request)
        {
            var validator = request.IsValid();
            if (!validator.Status)
            {
                throw new BadRequestException(validator.Message);
            }

            await DeleteAsync(request.Id);
            
            return new DeleteWorkspaceResponse()
            {
                StatusCode = (HttpStatusCode) 200,
                Message = "Success.",
            };
        }
    }
}