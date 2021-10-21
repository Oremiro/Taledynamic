using System.Threading.Tasks;
using Taledynamic.Core.Entities;
using Taledynamic.Core.Models.Requests.WorkspaceRequests;
using Taledynamic.Core.Models.Responses.WorkspaceResponses;

namespace Taledynamic.Core.Interfaces
{
    public interface IWorkspaceService
    {
        public Task<GetWorkspacesByUserResponse> GetFilteredByUserIdAsync(GetWorkspacesByUserRequest request,
            User user);
    }
}