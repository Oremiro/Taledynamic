using System;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Serilog;
using Taledynamic.Api.Attributes;
using Taledynamic.Core;
using Taledynamic.Core.Helpers;
using Taledynamic.Core.Interfaces;
using Taledynamic.DAL.Models.Requests.WorkspaceRequests;
using Taledynamic.DAL.Models.Responses.WorkspaceResponses;

namespace Taledynamic.Api.Controllers
{
    [ApiController]
    [JwtAuthorize]
    [Route("data/[controller]")]
    public class WorkspaceController: BaseController
    {
        private IWorkspaceService _workspaceService { get;  }
        public WorkspaceController(IWorkspaceService workspaceService)
        {
            _workspaceService = workspaceService;
        }

        [HttpGet("get-filtered-by-user")]
        public async Task<GetWorkspacesByUserResponse> GetFilteredByUser([FromQuery] GetWorkspacesByUserRequest request)
        {
            Log.Information($"[{nameof(WorkspaceController)}]: Method 'GetFilteredByUser' started.");
            var response = await _workspaceService.GetFilteredByUserIdAsync(request, CustomUser);
            Log.Information($"[{nameof(WorkspaceController)}]: Method 'GetFilteredByUser' ended.");
            return response;
        }
        
        [HttpGet("get")]
        public async Task<GetWorkspaceByIdResponse> Get([FromQuery] GetWorkspaceByIdRequest request)
        {
            Log.Information($"[{nameof(WorkspaceController)}]: Method 'Get' started.");
            var response = await _workspaceService.GetUserWorkspaceByIdAsync(request, CustomUser);
            Log.Information($"[{nameof(WorkspaceController)}]: Method 'Get' ended.");
            return response;
        }
        
        [HttpPost("create")]
        public async Task<CreateWorkspaceResponse> Create([FromBody] CreateWorkspaceRequest request)
        {
            Log.Information($"[{nameof(WorkspaceController)}]: Method 'Create' started.");
            var response = await _workspaceService.CreateWorkspaceAsync(request, CustomUser);
            Log.Information($"[{nameof(WorkspaceController)}]: Method 'Create' ended.");
            return response;
        }
        [HttpPut("update")]
        public async Task<UpdateWorkspaceResponse> Update([FromBody] UpdateWorkspaceRequest request)
        {
            Log.Information($"[{nameof(WorkspaceController)}]: Method 'Update' started.");
            var response = await _workspaceService.UpdateWorkspaceAsync(request);
            Log.Information($"[{nameof(WorkspaceController)}]: Method 'Update' ended.");
            return response;
        }
        [HttpDelete("delete")]
        public async Task<DeleteWorkspaceResponse> Delete([FromQuery] DeleteWorkspaceRequest request)
        {
            Log.Information($"[{nameof(WorkspaceController)}]: Method 'Delete' started.");
            var response = await _workspaceService.DeleteWorkspaceAsync(request);
            Log.Information($"[{nameof(WorkspaceController)}]: Method 'Delete' ended.");
            return response;
        }
    }
}