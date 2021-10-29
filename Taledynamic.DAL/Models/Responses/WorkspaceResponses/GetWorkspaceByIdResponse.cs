using Taledynamic.DAL.Entities;

namespace Taledynamic.DAL.Models.Responses.WorkspaceResponses
{
    public class GetWorkspaceByIdResponse: BaseResponse
    {
        public Workspace Workspace { get; set; }
    }
}