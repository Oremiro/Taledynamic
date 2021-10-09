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
    [ApiController]
    [Route("auth/user")]
    public class UserController: ControllerBase
    {
        private IUserService _userService { get; }
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("authenticate")]
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

        }
        
        [HttpGet("get-all")]
        public async Task<GetUsersResponse> GetAll()
        {
            return null;

        }
        [HttpGet("get/{id:int}")]
        public async Task<GetUserResponse> GetById(int id)
        {
            return null;

        }
        
        [HttpPut("update")]
        public async Task<UpdateUserRequest> Update([FromBody] UpdateUserRequest request)
        {
            return null;

        }
        
        [HttpDelete("delete")]
        public async Task<DeleteUserResponse> Delete([FromBody] DeleteUserRequest request)
        {
            return null;

        }
        
        [HttpPost("create")]
        public async Task<CreateUserResponse> Create([FromBody] CreateUserRequest request)
        {
            //CreateUserResponse response = await _userService.CreateAsync();
            return null;
        }
    }
}