using System.Threading.Tasks;
using Taledynamic.Core.Models.Requests;
using Taledynamic.Core.Models.Responses;

namespace Taledynamic.Core.Interfaces
{
    public interface IUserRepository
    {
        public Task<AuthenticateResponse> AuthenticateAsync(AuthenticateRequest model, string ipAddress);
        public Task<AuthenticateResponse> RefreshTokenAsync(string token, string ipAddress);
        public Task<bool> RevokeTokenAsync(string token, string ipAddress);
        
    }
}