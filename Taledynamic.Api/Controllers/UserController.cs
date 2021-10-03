using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Taledynamic.Core;
using Taledynamic.Core.Interfaces;
using Taledynamic.Core.Models.Requests;
using Taledynamic.Core.Models.Requests.UserRequests;
using Taledynamic.Core.Models.Responses;
using Taledynamic.Core.Models.Responses.UserResponses;

namespace Taledynamic.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("auth")]
    public class UserController: ControllerBase
    {
        public UserController(DbContext context)
        {
        }
        
        [HttpPost("revoke-token")]
        public async Task<RevokeTokenResponse> RevokeToken([FromBody] RevokeTokenRequest request)
        {
            throw new NotImplementedException();
        }

        public async Task IsEmailUsed()
        {
            
        }
        
        public async Task GetAll()
        {
            
        }
        
        public async Task GetById()
        {
            
        }
        
        public async Task Update()
        {
            
        }
        public async Task Delete()
        {
            
        }
        public async Task Create()
        {
            
        }
        

    }
}