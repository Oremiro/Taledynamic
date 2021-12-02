using System.Threading.Tasks;
using Taledynamic.DAL.Models.Requests.UserRequests;
using Taledynamic.DAL.Models.Responses.UserResponses;

namespace Taledynamic.Core.Interfaces
{
    public interface IUserService
    {
        public Task<AuthenticateResponse> AuthenticateAsync(AuthenticateRequest request, string ipAddress);
        public Task<RefreshTokenResponse> RefreshTokenAsync(string token, string ipAddress);
        public Task<RevokeTokenResponse> RevokeTokenAsync(string token, string ipAddress);
        public Task<CreateUserResponse> CreateUserAsync(CreateUserRequest request, string ipAddress);
        public Task<DeleteUserResponse> DeleteUserAsync(DeleteUserRequest request);
        public Task<UpdateUserResponse> UpdateUserAsync(UpdateUserRequest request);
        public Task<GetUserResponse> GetUserByIdAsync(GetUserRequest request);
        public Task<GetUsersResponse> GetUsersAsync(GetUsersRequest request);
        public Task<IsEmailUsedResponse> IsEmailUsedAsync(IsEmailUsedRequest request);
        public Task<GetUserResponse> GetActiveUserByEmailAsync(GetActiveUserByEmailRequest request);
        public Task<TgAuthResponse> TgAuthAsync(TgAuthRequest request);

    }
}