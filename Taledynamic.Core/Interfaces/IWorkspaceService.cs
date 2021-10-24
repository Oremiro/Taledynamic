using System.Threading.Tasks;
using Taledynamic.DAL.Entities;
using Taledynamic.DAL.Models.Requests.WorkspaceRequests;
using Taledynamic.DAL.Models.Responses.WorkspaceResponses;

namespace Taledynamic.Core.Interfaces
{
    public interface IWorkspaceService
    {
        public Task<GetWorkspacesByUserResponse> GetFilteredByUserIdAsync(GetWorkspacesByUserRequest request,
            User user);

        public Task<CreateWorkspaceResponse> CreateWorkspaceAsync(CreateWorkspaceRequest request, User user);
        public Task<GetWorkspaceByIdResponse> GetUserWorkspaceByIdAsync(GetWorkspaceByIdRequest request, User user);
        public Task<DeleteWorkspaceResponse> DeleteWorkspaceAsync(DeleteWorkspaceRequest request);
        public Task<UpdateWorkspaceResponse> UpdateWorkspaceAsync(UpdateWorkspaceRequest request);
    }
}