using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Taledynamic.Core.Interfaces;
using Taledynamic.Core;
using Taledynamic.Core.Entities;
using Taledynamic.Core.Helpers;
using Taledynamic.Core.Models.Requests;
using Taledynamic.Core.Models.Requests.UserRequests;
using Taledynamic.Core.Models.Responses;
using Taledynamic.Core.Models.Responses.UserResponses;

namespace Taledynamic.Core.Services
{
    public class UserService: BaseService<User>, IUserService
    {
        private TaledynamicContext _context { get; set; }
        private IOptions<AppSettings> _appSettings { get; set; }
        private UserHelper _userHelper { get; set; }
        public UserService(TaledynamicContext context, IOptions<AppSettings> appSettings): base(context)
        {
            _context = context;
            _appSettings = appSettings;
            _userHelper = new UserHelper(_appSettings);
        }
        public async Task<AuthenticateResponse> AuthenticateAsync(AuthenticateRequest model, string ipAddress)
        {
            var response = new AuthenticateResponse(user: null, jwtToken: null, refreshToken: null);

            User user = await _context
                .Users
                .AsQueryable()
                .AsNoTracking()
                .SingleOrDefaultAsync(u => u.Email == model.Email && u.Password == model.Password);

            if (user == null)
            {
                response.Message = "User is not found.";
                response.StatusCode = HttpStatusCode.NotFound;
                return response;
            }
            
            var jwtToken = _userHelper.GenerateJwtToken(user);
            var refreshToken = _userHelper.GenerateRefreshToken(ipAddress);
            
            user.RefreshTokens.Add(refreshToken);
            _context.Update(user);
            await _context.SaveChangesAsync();

            response = new AuthenticateResponse(user, jwtToken, refreshToken.Token)
            {
                Message = "Authenticate proccess ended with success.",
                StatusCode = HttpStatusCode.OK
            };
            return response;
        }

        public async Task<RefreshTokenResponse> RefreshTokenAsync(string token, string ipAddress)
        {
            var response = new RefreshTokenResponse(user: null, jwtToken: null, refreshToken: null)
            {
                StatusCode = HttpStatusCode.OK
            };

            var user = await _context
                .Users
                .AsQueryable()
                .SingleOrDefaultAsync(u => u.RefreshTokens.Any(t => t.Token == token));
            
            if (user == null)
            {
                response.Message = "User with token is not found.";
                response.StatusCode = HttpStatusCode.NotFound;
                return response;
            }

            var refreshToken = user.RefreshTokens.Single(x => x.Token == token);
            
            if (!refreshToken.IsActive)
            {
                response.Message = "Token is not active.";
                response.StatusCode = HttpStatusCode.NotFound;
                return response;
            }
            
            var newRefreshToken = _userHelper.GenerateRefreshToken(ipAddress);
            refreshToken.Revoked = DateTime.UtcNow;
            refreshToken.RevokedByIp = ipAddress;
            refreshToken.ReplacedByToken = newRefreshToken.Token;
            user.RefreshTokens.Add(newRefreshToken);
            _context.Update(user);
            await _context.SaveChangesAsync();
            
            var jwtToken = _userHelper.GenerateJwtToken(user);

            return new RefreshTokenResponse(user, jwtToken, newRefreshToken.Token);
        }

        public async Task<RevokeTokenResponse> RevokeTokenAsync(string token, string ipAddress)
        {
            var response = new RevokeTokenResponse
            {
                StatusCode = HttpStatusCode.OK
            };

            var user = await _context
                .Users
                .AsQueryable()
                .SingleOrDefaultAsync(u => u.RefreshTokens.Any(t => t.Token == token));
            
            if (user == null)
            {
                response.Message = "User with token is not found.";
                response.StatusCode = HttpStatusCode.NotFound;
                response.IsSuccess = false;
                return response;
                
            }

            var refreshToken = user.RefreshTokens.Single(x => x.Token == token);
            
            if (!refreshToken.IsActive)
            {
                response.Message = "Token is not active.";
                response.StatusCode = HttpStatusCode.NotFound;
                response.IsSuccess = false;
                return response;
            }

            refreshToken.Revoked = DateTime.UtcNow;
            refreshToken.RevokedByIp = ipAddress;
            _context.Update(user);
            await _context.SaveChangesAsync();

            response.IsSuccess = true;
            return response;
        }
    }
}