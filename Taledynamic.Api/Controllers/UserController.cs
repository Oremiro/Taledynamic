using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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
    [Route("auth/[controller]")]
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
            AuthenticateResponse response = await _userService.AuthenticateAsync(request, GetIpAddress());
            SetTokenCookie(response.RefreshToken);
            return response;
        }
        [HttpPost("refresh-token")]
        public async Task<RefreshTokenResponse> RefreshToken([FromBody] RefreshTokenRequest request)
        {
            //TODO change nulls
            RefreshTokenResponse response = await _userService.RefreshTokenAsync(null, GetIpAddress());
            SetTokenCookie(response.RefreshToken);
            return response;
        }

        [HttpPost("revoke-token")]
        public async Task<RevokeTokenResponse> RevokeToken([FromBody] RevokeTokenRequest request)
        {
            //TODO change nulls
            RevokeTokenResponse response = await _userService.RevokeTokenAsync(request.Token, GetIpAddress());
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
        
        [HttpPut("update/{id:int}")]
        public async Task<UpdateUserRequest> Update(int id, [FromBody] UpdateUserRequest request)
        {
            return null;
            

        }
        
        [HttpDelete("delete")]
        public async Task<DeleteUserResponse> Delete([FromQuery] DeleteUserRequest request)
        {
            return null;

        }
        
        [HttpPost("create")]
        public async Task<CreateUserResponse> Create([FromBody] CreateUserRequest request)
        {
            //CreateUserResponse response = await _userService.CreateAsync();
            return null;
        }

        private string GetRefreshTokenFromCookie()
        {
            string refreshToken = Request.Cookies["refreshToken"];
            return refreshToken;
        }
        private void SetTokenCookie(string token)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = DateTime.UtcNow.AddDays(7)
            };
            Response.Cookies.Append("refreshToken", token, cookieOptions);
        }

        private string GetIpAddress()
        {
            if (Request.Headers.ContainsKey("X-Forwarded-For"))
            {
                return Request.Headers["X-Forwarded-For"];
            }
            else
            {
                return HttpContext?.Connection?.RemoteIpAddress?.MapToIPv4().ToString() ?? "";
            }
        }
    }
}