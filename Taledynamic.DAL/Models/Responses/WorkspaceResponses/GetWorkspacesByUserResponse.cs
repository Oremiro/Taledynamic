using System.Collections.Generic;
using Taledynamic.DAL.Entities;

namespace Taledynamic.DAL.Models.Responses.WorkspaceResponses
{
    public class GetWorkspacesByUserResponse: BaseResponse
    {
        public List<Workspace> Workspaces { get; set; } 
    }
}