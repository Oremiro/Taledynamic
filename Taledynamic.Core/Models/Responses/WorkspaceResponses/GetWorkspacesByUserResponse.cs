using System.Collections.Generic;
using Taledynamic.Core.Entities;

namespace Taledynamic.Core.Models.Responses.WorkspaceResponses
{
    public class GetWorkspacesByUserResponse: BaseResponse
    {
        public List<Workspace> Workspaces { get; set; } 
    }
}