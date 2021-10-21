using Taledynamic.Core.Entities;

namespace Taledynamic.Core.Models.Responses.WorkspaceResponses
{
    public class GetWorkspaceByIdResponse: BaseResponse
    {
        public Workspace Workspace { get; set; }
    }
}