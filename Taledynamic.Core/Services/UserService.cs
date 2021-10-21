using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Taledynamic.Core.Interfaces;
using Taledynamic.Core.Entities;
using Taledynamic.Core.Exceptions;
using Taledynamic.Core.Helpers;
using Taledynamic.Core.Models.DTOs;
using Taledynamic.Core.Models.Requests.UserRequests;
using Taledynamic.Core.Models.Responses.UserResponses;

namespace Taledynamic.Core.Services
{
    public class UserService : BaseService<User>, IUserService
    {
        private TaledynamicContext _context { get; set; }
        private IOptions<AppSettings> _appSettings { get; set; }
        private UserHelper _userHelper { get; set; }

        public UserService(TaledynamicContext context, IOptions<AppSettings> appSettings) : base(context)
        {
            _context = context;
            _appSettings = appSettings;
            _userHelper = new UserHelper(_appSettings);
        }

        public async Task<AuthenticateResponse> AuthenticateAsync(AuthenticateRequest request, string ipAddress)
        {
            var response = new AuthenticateResponse();

            var validator = request.IsValid();
            if (!validator.Status)
            {
                throw new BadRequestException(validator.Message);
            }

            User user = await _context
                .Users
                .AsQueryable()
                .AsNoTracking()
                .SingleOrDefaultAsync(u => u.Email == request.Email && u.Password == request.Password);

            if (user == null)
            {
                throw new NotFoundException("User is not found.");
            }

            var jwtToken = _userHelper.GenerateJwtToken(user);
            var refreshToken = _userHelper.GenerateRefreshToken(ipAddress);

            user.RefreshTokens.Add(refreshToken);
            _context.Update(user);
            await _context.SaveChangesAsync();

            response = new AuthenticateResponse()
            {
                Id = user.Id,
                Email = user.Email,
                JwtToken = jwtToken,
                RefreshToken = refreshToken.Token,
                Message = "Authenticate proccess ended with success.",
                StatusCode = HttpStatusCode.OK
            };
            return response;
        }

        public async Task<RefreshTokenResponse> RefreshTokenAsync(string token, string ipAddress)
        {
            var response = new RefreshTokenResponse();

            var user = await _context
                .Users
                .AsQueryable()
                .SingleOrDefaultAsync(u => u.RefreshTokens.Any(t => t.Token == token));

            if (user == null)
            {
                throw new NotFoundException("User with token is not found.");
            }

            var refreshToken = user.RefreshTokens.Single(x => x.Token == token);

            if (!(refreshToken.Revoked == null || refreshToken.IsExpired))
            {
                throw new NotFoundException("Token is not active.");
            }

            var newRefreshToken = _userHelper.GenerateRefreshToken(ipAddress);
            if (newRefreshToken == null)
            {
                throw new InternalServerErrorException("New jwt token is not generated properly");
            }
            refreshToken.Revoked = DateTime.UtcNow;
            refreshToken.RevokedByIp = ipAddress;
            refreshToken.ReplacedByToken = newRefreshToken.Token;
            refreshToken.IsActive = false;
            user.RefreshTokens.Add(newRefreshToken);
            _context.Update(user);
            await _context.SaveChangesAsync();

            var jwtToken = _userHelper.GenerateJwtToken(user);

            return new RefreshTokenResponse
            {
                Id = user.Id,
                Email = user.Email,
                JwtToken = jwtToken,
                RefreshToken = newRefreshToken.Token,
                Message = "Refresh proccess ended with success.",
                StatusCode = HttpStatusCode.OK
            };
        }

        public async Task<RevokeTokenResponse> RevokeTokenAsync(string token, string ipAddress)
        {
            var response = new RevokeTokenResponse
            {
                StatusCode = HttpStatusCode.OK,
                IsSuccess = false
            };

            var user = await _context
                .Users
                .AsQueryable()
                .SingleOrDefaultAsync(u => u.RefreshTokens.Any(t => t.Token == token));

            if (user == null)
            {
                throw new NotFoundException("User with token is not found.");
            }

            var refreshToken = user.RefreshTokens.Single(x => x.Token == token);

            if (!(refreshToken.Revoked == null || refreshToken.IsExpired))
            {
                throw new NotFoundException("Token is not active.");
            }

            refreshToken.Revoked = DateTime.UtcNow;
            refreshToken.RevokedByIp = ipAddress;
            _context.Update(user);
            await _context.SaveChangesAsync();

            response.IsSuccess = true;
            response.Message = "Success.";
            return response;
        }

        public async Task<CreateUserResponse> CreateUserAsync(CreateUserRequest request, string ipAddress)
        {
            var validator = request.IsValid();
            if (!validator.Status)
            {
                throw new BadRequestException(validator.Message);
            }

            var isExist = await _context
                .Users
                .AsQueryable()
                .AnyAsync(u => u.Email == request.Email);

            if (isExist)
            {
                throw new BadRequestException("User with the same email already exist.");
            }

            User user = new User
            {
                IsActive = true,
                Email = request.Email,
                Password = request.Password,
                RefreshTokens = new List<RefreshToken>()
            };

            var refreshToken = _userHelper.GenerateRefreshToken(ipAddress);
            user.RefreshTokens.Add(refreshToken);
            await this.CreateAsync(user);
            var response = new CreateUserResponse
            {
                StatusCode = (HttpStatusCode) 200,
                Message = "Success."
            };

            return response;
        }

        public async Task<DeleteUserResponse> DeleteUserAsync(DeleteUserRequest request)
        {
            var validator = request.IsValid();
            if (!validator.Status)
            {
                throw new BadRequestException(validator.Message);
            }

            var userId = request.UserId;
            await this.DeleteAsync(userId);
            var response = new DeleteUserResponse()
            {
                StatusCode = (HttpStatusCode) 200,
                Message = "Success."
            };

            return response;
        }

        public async Task<UpdateUserResponse> UpdateUserAsync(UpdateUserRequest request)
        {
            var validator = request.IsValid();
            if (!validator.Status)
            {
                throw new BadRequestException(validator.Message);
            }

            var userId = request.Id;
            
            await using var transaction = await _context.Database.BeginTransactionAsync();
            var oldUser = await _context
                .Users
                .Include(u => u.RefreshTokens)
                .AsNoTracking()
                .SingleOrDefaultAsync(u => u.Id == userId);
            await transaction.CommitAsync();
            if (!oldUser.IsActive)
            {
                throw new BadRequestException("Nothing to update by this id.");
            }
            var refreshTokens = oldUser.RefreshTokens;
            _context.RefreshTokens.RemoveRange(refreshTokens);
            User newUser = new User
            {
                IsActive = true,
                Email = request.Email,
                Password = request.Password,
                RefreshTokens = refreshTokens
            };
            
            await this.CreateAsync(newUser);
            _context.ChangeTracker.Clear();
            await DeleteAsync(userId);
            
            var response = new UpdateUserResponse()
            {
                StatusCode = (HttpStatusCode) 200,
                Message = "Success."
            };

            return response;
        }

        public async Task<GetUserResponse> GetUserByIdAsync(GetUserRequest request)
        {
            var validator = request.IsValid();
            if (!validator.Status)
            {
                throw new BadRequestException(validator.Message);
            }

            var userId = request.Id;
            var user = await this.GetByIdAsync(userId);

            if (user == null)
            {
                throw new NotFoundException("User is not found.");
            }

            var response = new GetUserResponse()
            {
                StatusCode = (HttpStatusCode) 200,
                Message = "Success.",
                UserDto = new GetUserDto
                {
                    Email = user.Email,
                    Id = user.Id
                }
            };

            return response;
        }

        public async Task<GetUsersResponse> GetUsersAsync(GetUsersRequest request)
        {
            var users = (await this.GetAllAsync()).Select(u => new GetUserDto
            {
                Id = u.Id,
                Email = u.Email
            }).ToList();


            var response = new GetUsersResponse()
            {
                StatusCode = (HttpStatusCode) 200,
                Message = "Success.",
                Users = users
            };

            return response;
        }

        public async Task<IsEmailUsedResponse> IsEmailUsedAsync(IsEmailUsedRequest request)
        {
            var validator = request.IsValid();
            if (!validator.Status)
            {
                throw new BadRequestException(validator.Message);
            }

            var user = await _context
                .Users
                .AsQueryable()
                .SingleOrDefaultAsync(u => u.Email == request.Email);

            var response = new IsEmailUsedResponse()
            {
                StatusCode = (HttpStatusCode) 200,
                Message = "Success.",
                IsEmailUsed = user != null
            };

            return response;
        }

        public async Task<GetUserResponse> GetActiveUserByEmailAsync(GetActiveUserByEmailRequest request)
        {
            var validator = request.IsValid();
            if (!validator.Status)
            {
                throw new BadRequestException(validator.Message);
            }

            var user = await _context
                .Users
                .AsQueryable()
                .SingleOrDefaultAsync(u => u.Email == request.Email && u.IsActive);

            if (user == null)
            {
                throw new NotFoundException("User is not found.");
            }

            GetUserDto userDto = new GetUserDto
            {
                Email = user.Email,
                Id = user.Id,
            };


            var response = new GetUserResponse()
            {
                StatusCode = (HttpStatusCode) 200,
                Message = "User was found.",
                UserDto = userDto
            };

            return response;
        }
    }
}