using System.Threading.Tasks;
using Taledynamic.Core.Entities;
using Taledynamic.Core.Models.Requests;
using Taledynamic.Core.Models.Requests.UserRequests;
using Taledynamic.Core.Models.Responses;
using Taledynamic.Core.Models.Responses.UserResponses;

namespace Taledynamic.Core.Interfaces
{
    public interface IUserService
    {
        public Task<AuthenticateResponse> AuthenticateAsync(AuthenticateRequest model, string ipAddress);
        public Task<RefreshTokenResponse> RefreshTokenAsync(string token, string ipAddress);
        public Task<RevokeTokenResponse> RevokeTokenAsync(string token, string ipAddress);
        public Task<CreateUserResponse> CreateUserAsync(CreateUserRequest request);
        public Task<DeleteUserResponse> DeleteUserAsync(DeleteUserRequest request);
        public Task<UpdateUserResponse> UpdateUserAsync(UpdateUserRequest request);
        public Task<GetUserResponse> GetUserAsync(GetUserRequest request);
        public Task<GetUsersResponse> GetUsersAsync(GetUsersRequest request);
        public Task<IsEmailUsedResponse> IsEmailUsedAsync(IsEmailUsedRequest request);
    }
}