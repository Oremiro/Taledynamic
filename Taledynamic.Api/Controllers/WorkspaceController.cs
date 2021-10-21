using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Taledynamic.Api.Attributes;
using Taledynamic.Core;
using Taledynamic.Core.Helpers;
using Taledynamic.Core.Interfaces;
using Taledynamic.Core.Models.Requests.WorkspaceRequests;
using Taledynamic.Core.Models.Responses.WorkspaceResponses;

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
        public async Task<GetWorkspacesByUserResponse> GetFilteredByUfer([FromQuery] GetWorkspacesByUserRequest request)
        {
            var response = await _workspaceService.GetFilteredByUserIdAsync(request, CustomUser);
            return response;
        }
        
        [HttpGet("get")]
        public async Task<GetWorkspaceByIdResponse> Get([FromQuery] GetWorkspaceByIdRequest request)
        {
            var response = await _workspaceService.GetUserWorkspaceByIdAsync(request, CustomUser);
            return response;
        }
        
        [HttpPost("create")]
        public async Task<CreateWorkspaceResponse> Create([FromBody] CreateWorkspaceRequest request)
        {
            var response = await _workspaceService.CreateWorkspaceAsync(request, CustomUser);
            return response;
        }
        [HttpPut("update")]
        public async Task<UpdateWorkspaceResponse> Update([FromBody] UpdateWorkspaceRequest request)
        {
            var response = await _workspaceService.UpdateWorkspaceAsync(request);
            return response;
        }
        [HttpDelete("delete")]
        public async Task<DeleteWorkspaceResponse> Delete([FromQuery] DeleteWorkspaceRequest request)
        {
            var response = await _workspaceService.DeleteWorkspaceAsync(request);
            return response;
        }
    }
}