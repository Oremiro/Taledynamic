using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Taledynamic.Core;
using Taledynamic.Core.Interfaces;
using Taledynamic.Core.Models.Requests;
using Taledynamic.Core.Models.Responses;
using Taledynamic.Core.Operations.User;

namespace Taledynamic.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("auth")]
    public class UserController: ControllerBase
    {
        [HttpPost("revoke-token")]
        public async Task<RevokeTokenResponse> RevokeToken([FromBody] RevokeTokenRequest request)
        {
           var executor = new OperationExecutor<RevokeTokenRequest, RevokeTokenResponse>(new RevokeTokenOperation());
           var response = await executor.ExecuteAsync(request);
           return response;
        }

    }
}