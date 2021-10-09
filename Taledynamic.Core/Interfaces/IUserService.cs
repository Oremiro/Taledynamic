using System.Threading.Tasks;
using Taledynamic.Core.Entities;
using Taledynamic.Core.Models.Requests;
using Taledynamic.Core.Models.Requests.UserRequests;
using Taledynamic.Core.Models.Responses;
using Taledynamic.Core.Models.Responses.UserResponses;

namespace Taledynamic.Core.Interfaces
{
    public interface IUserService: IBaseService<User>
    {
        public Task<AuthenticateResponse> AuthenticateAsync(AuthenticateRequest model, string ipAddress);
        public Task<RefreshTokenResponse> RefreshTokenAsync(string token, string ipAddress);
        public Task<RevokeTokenResponse> RevokeTokenAsync(string token, string ipAddress);
    }
}