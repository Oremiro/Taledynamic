using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Taledynamic.Core.Interfaces;
using Taledynamic.Core;
using Taledynamic.Core.Entities;
using Taledynamic.Core.Helpers;
using Taledynamic.Core.Models.DTOs;
using Taledynamic.Core.Models.Internal;
using Taledynamic.Core.Models.Requests;
using Taledynamic.Core.Models.Requests.UserRequests;
using Taledynamic.Core.Models.Responses;
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

        public async Task<AuthenticateResponse> AuthenticateAsync(AuthenticateRequest model, string ipAddress)
        {
            try
            {
                var response = new AuthenticateResponse();

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
            catch (Exception e)
            {
                return new AuthenticateResponse()
                {
                    StatusCode = (HttpStatusCode) 400,
                    Message = $"There was an exception in method \"AuthenticateAsync\". Stacktrace - {e.StackTrace}"
                };
            }
        }

        public async Task<RefreshTokenResponse> RefreshTokenAsync(string token, string ipAddress)
        {
            try
            {
                var response = new RefreshTokenResponse();

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

                return new RefreshTokenResponse
                {
                    Id = user.Id,
                    Email = user.Email,
                    JwtToken = jwtToken,
                    RefreshToken = refreshToken.Token,
                    Message = "Refresh proccess ended with success.",
                    StatusCode = HttpStatusCode.OK
                };
            }
            catch (Exception e)
            {
                return new RefreshTokenResponse
                {
                    StatusCode = (HttpStatusCode) 400,
                    Message = $"There was an exception in method \"RefreshTokenAsync\". Stacktrace - {e.StackTrace}"
                };
            }
        }

        public async Task<RevokeTokenResponse> RevokeTokenAsync(string token, string ipAddress)
        {
            try
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
            catch (Exception e)
            {
                return new RevokeTokenResponse()
                {
                    StatusCode = (HttpStatusCode) 400,
                    IsSuccess = false,
                    Message = $"There was an exception in method \"RevokeTokenAsync\". Stacktrace - {e.StackTrace}"
                };
            }
        }

        public async Task<CreateUserResponse> CreateUserAsync(CreateUserRequest request, string ipAddress)
        {
            try
            {
                var validator = ValidateCreateRequest(request);
                if (!validator.Status)
                {
                    return new CreateUserResponse
                    {
                        StatusCode = (HttpStatusCode) 400,
                        Message = validator.Message
                    };
                }

                var isExist = await _context
                    .Users
                    .AsQueryable()
                    .AnyAsync(u => u.Email == request.Email);

                if (isExist)
                {
                    return new CreateUserResponse
                    {
                        StatusCode = (HttpStatusCode) 400,
                        Message = "User with the same email already exist."
                    };
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
                    Message = "User was created successfully"
                };

                return response;
            }
            catch (Exception e)
            {
                return new CreateUserResponse
                {
                    StatusCode = (HttpStatusCode) 400,
                    Message = $"There was an exception in method \"CreateUserAsync\". Stacktrace - {e.StackTrace}"
                };
            }
        }

        public async Task<DeleteUserResponse> DeleteUserAsync(DeleteUserRequest request)
        {
            try
            {
                var validator = ValidateDeleteUserRequest(request);
                if (!validator.Status)
                {
                    return new DeleteUserResponse()
                    {
                        StatusCode = (HttpStatusCode) 400,
                        Message = validator.Message
                    };
                }

                var userId = request.UserId;
                await this.DeleteAsync(userId);
                var response = new DeleteUserResponse()
                {
                    StatusCode = (HttpStatusCode) 200,
                    Message = "User was deleted successfully"
                };

                return response;
            }
            catch (Exception e)
            {
                return new DeleteUserResponse()
                {
                    StatusCode = (HttpStatusCode) 400,
                    Message = $"There was an exception in method \"DeleteUserAsync\". Stacktrace - {e.StackTrace}"
                };
            }
        }

        public async Task<UpdateUserResponse> UpdateUserAsync(UpdateUserRequest request)
        {
            try
            {
                var validator = ValidateUpdateUserRequest(request);
                if (!validator.Status)
                {
                    return new UpdateUserResponse()
                    {
                        StatusCode = (HttpStatusCode) 400,
                        Message = validator.Message
                    };
                }

                var userId = request.Id;
                var oldUser = await _context
                    .Users
                    .AsNoTracking()
                    .Include(u => u.RefreshTokens)
                    .SingleOrDefaultAsync(u => u.Id == userId);

                if (!oldUser.IsActive)
                {
                    return new UpdateUserResponse()
                    {
                        StatusCode = HttpStatusCode.NoContent,
                        Message = "Nothing to update by this id."
                    };
                }

                await DeleteAsync(userId);

                var refreshTokens = oldUser.RefreshTokens;
                User newUser = new User
                {
                    IsActive = true,
                    Email = request.Email,
                    Password = request.Password,
                    RefreshTokens = refreshTokens
                };

                await this.CreateAsync(newUser);
                var response = new UpdateUserResponse()
                {
                    StatusCode = (HttpStatusCode) 200,
                    Message = "User was updated successfully"
                };

                return response;
            }
            catch (Exception e)
            {
                return new UpdateUserResponse()
                {
                    StatusCode = (HttpStatusCode) 400,
                    Message = $"There was an exception in method \"UpdateUserAsync\". Stacktrace - {e.StackTrace}"
                };
            }
        }

        public async Task<GetUserResponse> GetUserByIdAsync(GetUserRequest request)
        {
            try
            {
                var validator = ValidateGetUserRequest(request);
                if (!validator.Status)
                {
                    return new GetUserResponse()
                    {
                        StatusCode = (HttpStatusCode) 400,
                        Message = validator.Message
                    };
                }

                var userId = request.Id;
                var user = await this.GetByIdAsync(userId);

                if (user == null)
                {
                    return new GetUserResponse
                    {
                        Message = "User is not found.",
                        StatusCode = HttpStatusCode.NotFound
                    };
                }
                
                var response = new GetUserResponse()
                {
                    StatusCode = (HttpStatusCode) 200,
                    Message = "User was got successfully",
                    UserDto = new GetUserDto
                    {
                        Email = user.Email,
                        Id = user.Id
                    }
                };

                return response;
            }
            catch (Exception e)
            {
                return new GetUserResponse()
                {
                    StatusCode = (HttpStatusCode) 400,
                    Message = $"There was an exception in method \"GetUserByIdAsync\". Stacktrace - {e.StackTrace}"
                };
            }
        }

        public async Task<GetUsersResponse> GetUsersAsync(GetUsersRequest request)
        {
            try
            {
                var users = (await this.GetAllAsync()).Select(u => new GetUserDto
                {
                    Id = u.Id,
                    Email = u.Email
                }).ToList();

                
                var response = new GetUsersResponse()
                {
                    StatusCode = (HttpStatusCode) 200,
                    Message = "Users was found successfully",
                    Users = users
                };

                return response;
            }
            catch (Exception e)
            {
                return new GetUsersResponse()
                {
                    StatusCode = (HttpStatusCode) 400,
                    Message = $"There was an exception in method \"GetUsersAsync\". Stacktrace - {e.StackTrace}"
                };
            }
        }

        public async Task<IsEmailUsedResponse> IsEmailUsedAsync(IsEmailUsedRequest request)
        {
            try
            {
                var validator = ValidateIsEmailUsedRequest(request);
                if (!validator.Status)
                {
                    return new IsEmailUsedResponse()
                    {
                        StatusCode = (HttpStatusCode) 400,
                        Message = validator.Message
                    };
                }

                var user = await _context
                    .Users
                    .AsQueryable()
                    .SingleOrDefaultAsync(u => u.Email == request.Email);

                var response = new IsEmailUsedResponse()
                {
                    StatusCode = (HttpStatusCode) 200,
                    Message = "Method ended without errors.",
                    IsEmailUsed = user != null
                };

                return response;
            }
            catch (Exception e)
            {
                return new IsEmailUsedResponse()
                {
                    StatusCode = (HttpStatusCode) 400,
                    Message = $"There was an exception in method \"IsEmailUsedAsync\". Stacktrace - {e.StackTrace}"
                };
            }
        }

        public async Task<GetUserResponse> GetActiveUserByEmailAsync(GetActiveUserByEmailRequest request)
        {
            try
            {
                var validator = ValidateGetActiveUserByEmailRequest(request);
                if (!validator.Status)
                {
                    return new GetUserResponse()
                    {
                        StatusCode = (HttpStatusCode) 400,
                        Message = validator.Message
                    };
                }

                var user = await _context
                    .Users
                    .AsQueryable()
                    .SingleOrDefaultAsync(u => u.Email == request.Email);

                if (user == null)
                {
                    return new GetUserResponse
                    {
                        Message = "User is not found.",
                        StatusCode = HttpStatusCode.NotFound
                    };
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
            catch (Exception e)
            {
                return new GetUserResponse()
                {
                    StatusCode = (HttpStatusCode) 400,
                    Message =
                        $"There was an exception in method \"GetActiveUserByEmailAsync\". Stacktrace - {e.StackTrace}"
                };
            }
        }

        private ValidateState ValidateCreateRequest(CreateUserRequest request)
        {
            StringBuilder sb = new StringBuilder();

            if (request.Email == null)
            {
                sb.Append("Email is not set.");
                ;
            }

            if (request.Password == null || request.ConfirmPassword == null)
            {
                sb.Append("Password is not set.");
            }

            if (sb.Length != 0)
            {
                return new ValidateState(false, sb.ToString());
            }

            return new ValidateState(true, "Success");
        }

        private ValidateState ValidateUpdateUserRequest(UpdateUserRequest request)
        {
            StringBuilder sb = new StringBuilder();

            if (request.Email == null)
            {
                sb.Append("Email is not set.");
            }

            if (request.Password == null || request.ConfirmPassword == null)
            {
                sb.Append("Password is not set.");
            }

            if (sb.Length != 0)
            {
                return new ValidateState(false, sb.ToString());
            }

            return new ValidateState(true, "Success");
        }

        private ValidateState ValidateDeleteUserRequest(DeleteUserRequest request)
        {
            StringBuilder sb = new StringBuilder();

            if (request.UserId == default)
            {
                sb.Append("UserId is default.");
            }

            if (sb.Length != 0)
            {
                return new ValidateState(false, sb.ToString());
            }

            return new ValidateState(true, "Success");
        }

        private ValidateState ValidateGetUserRequest(GetUserRequest request)
        {
            StringBuilder sb = new StringBuilder();

            if (request.Id == default)
            {
                sb.Append("UserId is default.");
            }

            if (sb.Length != 0)
            {
                return new ValidateState(false, sb.ToString());
            }

            return new ValidateState(true, "Success");
        }

        private ValidateState ValidateIsEmailUsedRequest(IsEmailUsedRequest request)
        {
            StringBuilder sb = new StringBuilder();
            if (request.Email == null)
            {
                sb.Append("UserId is default.");
            }

            if (sb.Length == 0)
            {
                return new ValidateState(false, sb.ToString());
            }

            return new ValidateState(true, "Success");
        }

        private ValidateState ValidateGetActiveUserByEmailRequest(GetActiveUserByEmailRequest request)
        {
            StringBuilder sb = new StringBuilder();

            if (string.IsNullOrEmpty(request.Email))
            {
                sb.Append("Email is empty.");
            }

            if (sb.Length != 0)
            {
                return new ValidateState(false, sb.ToString());
            }

            return new ValidateState(true, "Success");
        }
    }
}