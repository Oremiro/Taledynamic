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
        public async Task<GetWorkspacesByUserResponse> GetFilteredByUserAsync([FromQuery] GetWorkspacesByUserRequest request)
        {
            var response = await _workspaceService.GetFilteredByUserIdAsync(request, CustomUser);
            return response;
        }
        
        [HttpGet("get")]
        public async Task GetByIdAsync()
        {
            throw new NotImplementedException();
        }
        
        [HttpPost("create")]
        public async Task<CreateWorkspaceResponse> CreateWorkspaceAsync([FromBody] CreateWorkspaceRequest request)
        {
            throw new NotImplementedException();
        }
        [HttpPut("update")]
        public async Task UpdateAsync()
        {
            throw new NotImplementedException();
        }
        [HttpDelete("delete")]
        public async Task DeleteAsync()
        {
            throw new NotImplementedException();
        }
    }
}