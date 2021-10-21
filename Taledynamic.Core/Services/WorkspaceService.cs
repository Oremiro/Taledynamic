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

        public async Task<GetWorkspaceByIdResponse> GetWorkspaceByIdAsync(GetWorkspaceByIdRequest request)
        {
            try
            {
                throw new NotImplementedException();
            }
            catch (Exception exception)
            {
                throw new NotImplementedException();
            }
        }

        public async Task<CreateWorkspaceResponse> CreateWorkspaceAsync(CreateWorkspaceRequest request)
        {
            try
            {
                throw new NotImplementedException();
            }
            catch (Exception exception)
            {
                throw new NotImplementedException();
            }
        }

        public async Task<UpdateWorkspaceResponse> UpdateWorkspaceAsync(UpdateWorkspaceRequest request)
        {
            try
            {
                throw new NotImplementedException();
            }
            catch (Exception exception)
            {
                throw new NotImplementedException();
            }
        }

        public async Task<DeleteWorkspaceResponse> DeleteWorkspaceAsync(DeleteWorkspaceRequest request)
        {
            try
            {
                throw new NotImplementedException();
            }
            catch (Exception exception)
            {
                throw new NotImplementedException();
            }
        }
    }
}