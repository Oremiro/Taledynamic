using Taledynamic.DAL.Models.DTOs;

namespace Taledynamic.DAL.Models.Responses.WorkspaceResponses
{
    public class UpdateWorkspaceResponse: BaseResponse
    {
        public WorkspaceDto Workspace { get; set; }
    }
}