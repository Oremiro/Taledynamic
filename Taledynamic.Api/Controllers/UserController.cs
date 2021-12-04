using System;
using System.Collections.Generic;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Taledynamic.Api.Attributes;
using Taledynamic.Core;
using Taledynamic.Core.Interfaces;
using Taledynamic.DAL.Models.Requests.UserRequests;
using Taledynamic.DAL.Models.Responses.UserResponses;

namespace Taledynamic.Api.Controllers
{
    [ApiController]
    [Route("auth/[controller]")]
    public class UserController: BaseController
    {
        private IUserService _userService { get; }
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("authenticate")]
        public async Task<AuthenticateResponse> Authenticate([FromBody] AuthenticateRequest request)
        {
            Log.Information($"[{nameof(UserController)}]: Method '{MethodBase.GetCurrentMethod()?.Name}' started.");
            AuthenticateResponse response = await _userService.AuthenticateAsync(request, GetIpAddress());
            SetTokenCookie(response);
            Log.Information($"[{nameof(UserController)}]: Method '{MethodBase.GetCurrentMethod()?.Name}' ended.");
            return response;
        }
        
        [HttpPost("refresh-token")]
        public async Task<RefreshTokenResponse> RefreshToken()
        {
            Log.Information($"[{nameof(UserController)}]: Method '{MethodBase.GetCurrentMethod()?.Name}' started.");
            var token = GetRefreshTokenFromCookie();
            RefreshTokenResponse response = await _userService.RefreshTokenAsync(token, GetIpAddress());
            SetTokenCookie(response);
            Log.Information($"[{nameof(UserController)}]: Method '{MethodBase.GetCurrentMethod()?.Name}' ended.");
            return response;
        }

        [JwtAuthorize]
        [HttpPost("revoke-token")]
        public async Task<RevokeTokenResponse> RevokeToken([FromBody] RevokeTokenRequest request)
        {
            Log.Information($"[{nameof(UserController)}]: Method '{MethodBase.GetCurrentMethod()?.Name}' started.");
            var token = GetRefreshTokenFromCookie();
            token = request.RefreshToken ?? token;
            RevokeTokenResponse response = await _userService.RevokeTokenAsync(token, GetIpAddress());
            Log.Information($"[{nameof(UserController)}]: Method '{MethodBase.GetCurrentMethod()?.Name}' ended.");
            return response;
        }

        //TODO:[JwtAuthorize]
        [HttpGet("is-email-used")]
        public async Task<IsEmailUsedResponse> IsEmailUsed([FromQuery] IsEmailUsedRequest request)
        {
            Log.Information($"[{nameof(UserController)}]: Method '{MethodBase.GetCurrentMethod()?.Name}' started.");
            var response = await _userService.IsEmailUsedAsync(request);
            Log.Information($"[{nameof(UserController)}]: Method '{MethodBase.GetCurrentMethod()?.Name}' ended.");
            return response;
        }
        
        [JwtAuthorize]
        [HttpGet("get-by-email")]
        public async Task<GetUserResponse> GetActiveUserByEmail([FromQuery] GetActiveUserByEmailRequest request)
        {
            Log.Information($"[{nameof(UserController)}]: Method '{MethodBase.GetCurrentMethod()?.Name}' started.");
            var response = await _userService.GetActiveUserByEmailAsync(request);
            Log.Information($"[{nameof(UserController)}]: Method '{MethodBase.GetCurrentMethod()?.Name}' ended.");
            return response;
        }
        
        [JwtAuthorize]
        [HttpGet("get-all")]
        public async Task<GetUsersResponse> GetAll([FromQuery] GetUsersRequest request)
        {
            Log.Information($"[{nameof(UserController)}]: Method '{MethodBase.GetCurrentMethod()?.Name}' started.");
            var response = await _userService.GetUsersAsync(request);
            Log.Information($"[{nameof(UserController)}]: Method '{MethodBase.GetCurrentMethod()?.Name}' ended.");
            return response;

        }
        
        [JwtAuthorize]
        [HttpGet("get")]
        public async Task<GetUserResponse> GetById([FromQuery] GetUserRequest request)
        {
            Log.Information($"[{nameof(UserController)}]: Method '{MethodBase.GetCurrentMethod()?.Name}' started.");
            var response = await _userService.GetUserByIdAsync(request);
            Log.Information($"[{nameof(UserController)}]: Method '{MethodBase.GetCurrentMethod()?.Name}' ended.");
            return response;

        }
        
        [JwtAuthorize]
        [HttpPut("update")]
        public async Task<UpdateUserResponse> Update([FromBody] UpdateUserRequest request)
        {
            Log.Information($"[{nameof(UserController)}]: Method '{MethodBase.GetCurrentMethod()?.Name}' started.");
            var response = await _userService.UpdateUserAsync(request);
            Log.Information($"[{nameof(UserController)}]: Method '{MethodBase.GetCurrentMethod()?.Name}' ended.");
            return response;
        }
        
        [JwtAuthorize]
        [HttpDelete("delete")]
        public async Task<DeleteUserResponse> Delete([FromQuery] DeleteUserRequest request)
        {
            Log.Information($"[{nameof(UserController)}]: Method '{MethodBase.GetCurrentMethod()?.Name}' started.");
            var response = await _userService.DeleteUserAsync(request);
            Log.Information($"[{nameof(UserController)}]: Method '{MethodBase.GetCurrentMethod()?.Name}' ended.");
            return response;

        }
        
        //TODO : [JwtAuthorize]
        [HttpPost("create")]
        public async Task<CreateUserResponse> Create([FromBody] CreateUserRequest request)
        {
            Log.Information($"[{nameof(UserController)}]: Method '{MethodBase.GetCurrentMethod()?.Name}' started.");
            var ipAddress = GetIpAddress();
            var response = await _userService.CreateUserAsync(request, ipAddress);
            Log.Information($"[{nameof(UserController)}]: Method '{MethodBase.GetCurrentMethod()?.Name}' ended.");
            return response;
        }

        private string GetRefreshTokenFromCookie()
        {
            Log.Information($"[{nameof(UserController)}]: Method '{MethodBase.GetCurrentMethod()?.Name}' started.");
            string refreshToken = Request.Cookies["refreshToken"];
            Log.Information($"[{nameof(UserController)}]: Method '{MethodBase.GetCurrentMethod()?.Name}' ended.");
            return refreshToken;
        }
        private void SetTokenCookie(AuthenticateResponse response)
        {
            Log.Information($"[{nameof(UserController)}]: Method '{MethodBase.GetCurrentMethod()?.Name}' started.");
            
            if (response == null || string.IsNullOrEmpty(response.RefreshToken))
            {
                return;
            }

            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                SameSite = SameSiteMode.Lax,
                Expires = DateTime.UtcNow.AddDays(7)
            };
            Response.Cookies.Append("refreshToken", response.RefreshToken, cookieOptions);
            
            Log.Information($"[{nameof(UserController)}]: Method '{MethodBase.GetCurrentMethod()?.Name}' ended.");
        }

        private string GetIpAddress()
        {
            Log.Information($"[{nameof(UserController)}]: Method '{MethodBase.GetCurrentMethod()?.Name}' started.");
            
            if (Request.Headers.ContainsKey("X-Forwarded-For"))
            {
                Log.Information($"[{nameof(UserController)}]: Method '{MethodBase.GetCurrentMethod()?.Name}' ended.");
                
                return Request.Headers["X-Forwarded-For"];
            }
            else
            {
                Log.Information($"[{nameof(UserController)}]: Method '{MethodBase.GetCurrentMethod()?.Name}' ended.");
                
                return HttpContext?.Connection?.RemoteIpAddress?.MapToIPv4().ToString() ?? "";
            }
        }
    }
}