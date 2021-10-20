using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Taledynamic.Api.Attributes;
using Taledynamic.Core;
using Taledynamic.Core.Entities;
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
            SetTokenCookie(response);
            return response;
        }
        
        [HttpPost("refresh-token")]
        public async Task<RefreshTokenResponse> RefreshToken()
        {
            var token = GetRefreshTokenFromCookie();
            RefreshTokenResponse response = await _userService.RefreshTokenAsync(token, GetIpAddress());
            SetTokenCookie(response);
            return response;
        }

        [JwtAuthorize]
        [HttpPost("revoke-token")]
        public async Task<RevokeTokenResponse> RevokeToken([FromBody] RevokeTokenRequest request)
        {
            RevokeTokenResponse response = await _userService.RevokeTokenAsync(request.RefreshToken, GetIpAddress());
            return response;
        }

        [JwtAuthorize]
        [HttpGet("is-email-used")]
        public async Task<IsEmailUsedResponse> IsEmailUsed([FromQuery] IsEmailUsedRequest request)
        {
            var response = await _userService.IsEmailUsedAsync(request);
            return response;
        }
        
        [JwtAuthorize]
        [HttpGet("get-by-email")]
        public async Task<GetUserResponse> GetActiveUserByEmail([FromQuery] GetActiveUserByEmailRequest request)
        {
            var response = await _userService.GetActiveUserByEmailAsync(request);
            return response;
        }
        
        [JwtAuthorize]
        [HttpGet("get-all")]
        public async Task<GetUsersResponse> GetAll([FromQuery] GetUsersRequest request)
        {
            var response = await _userService.GetUsersAsync(request);
            return response;

        }
        
        [JwtAuthorize]
        [HttpGet("get")]
        public async Task<GetUserResponse> GetById([FromQuery] GetUserRequest request)
        {
            var response = await _userService.GetUserByIdAsync(request);
            return response;

        }
        
        [JwtAuthorize]
        [HttpPut("update")]
        public async Task<UpdateUserResponse> Update([FromBody] UpdateUserRequest request)
        {
            var response = await _userService.UpdateUserAsync(request);
            return response;
        }
        
        [JwtAuthorize]
        [HttpDelete("delete")]
        public async Task<DeleteUserResponse> Delete([FromQuery] DeleteUserRequest request)
        {
            
            var response = await _userService.DeleteUserAsync(request);
            return response;

        }
        
        [JwtAuthorize]
        [HttpPost("create")]
        public async Task<CreateUserResponse> Create([FromBody] CreateUserRequest request)
        {
            var ipAddress = GetIpAddress();
            var response = await _userService.CreateUserAsync(request, ipAddress);
            return response;
        }

        private string GetRefreshTokenFromCookie()
        {
            string refreshToken = Request.Cookies["refreshToken"];
            return refreshToken;
        }
        private void SetTokenCookie(AuthenticateResponse response)
        {
            if (response == null || string.IsNullOrEmpty(response.RefreshToken))
            {
                return;
            }

            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = DateTime.UtcNow.AddDays(7)
            };
            Response.Cookies.Append("refreshToken", response.RefreshToken, cookieOptions);
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