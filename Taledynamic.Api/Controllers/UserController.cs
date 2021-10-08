using System;
using System.Collections.Generic;
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
        private IUserService _userService { get; }
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        public async Task<AuthenticateResponse> Authenticate([FromBody] AuthenticateRequest request)
        {
            //TODO change nulls
            AuthenticateResponse response = await _userService.AuthenticateAsync(request, "");
            return response;
        }
        [HttpPost("refresh-token")]
        public async Task<RefreshTokenResponse> RefreshToken([FromBody] RefreshTokenRequest request)
        {
            //TODO change nulls
            RefreshTokenResponse response = await _userService.RefreshTokenAsync(null, null);
            return response;
        }

        [HttpPost("revoke-token")]
        public async Task<RevokeTokenResponse> RevokeToken([FromBody] RevokeTokenRequest request)
        {
            //TODO change nulls
            RevokeTokenResponse response = await _userService.RevokeTokenAsync(request.Token, "");
            return response;
        }

        [HttpGet("is-email-used/{email}")]
        public async Task IsEmailUsed(string email)
        {
            throw new NotImplementedException();

        }
        
        [HttpGet("get-all")]
        public async Task<GetUsersResponse> GetAll()
        {   
            throw new NotImplementedException();

        }
        [HttpGet("get/{id:int}")]
        public async Task<GetUserResponse> GetById(int id)
        {
            throw new NotImplementedException();

        }
        
        [HttpPut("update")]
        public async Task<UpdateUserRequest> Update([FromBody] UpdateUserRequest request)
        {
            throw new NotImplementedException();

        }
        
        [HttpDelete("delete")]
        public async Task<DeleteUserResponse> Delete([FromBody] DeleteUserRequest request)
        {
            throw new NotImplementedException();

        }
        
        [HttpPost("create")]
        public async Task<CreateUserResponse> Create([FromBody] CreateUserRequest request)
        {
            //CreateUserResponse response = await _userService.CreateAsync();
            throw new NotImplementedException();
        }
    }
}