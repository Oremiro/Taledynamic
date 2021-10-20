using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Taledynamic.Core.Entities;
using Taledynamic.Core.Models.Requests.WorkspaceRequests;
using Taledynamic.Core.Models.Responses.WorkspaceResponses;

namespace Taledynamic.Core.Services
{
    public class WorkspaceService: BaseService<Workspace>
    {
        private TaledynamicContext _context { get; set; }
        public WorkspaceService(TaledynamicContext context) : base(context)
        {
            _context = context;
        }

        public async Task<GetWorkspacesByUserIdResponse> GetFilteredByUserIdAsync(GetWorkspacesByUserIdRequest request)
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