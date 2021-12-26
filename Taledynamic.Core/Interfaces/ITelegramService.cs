using System.Threading.Tasks;
using Taledynamic.DAL.Entities;
using Taledynamic.DAL.Models.DTOs;
using Taledynamic.DAL.Models.Requests.TelegramRequests;
using Taledynamic.DAL.Models.Responses;

namespace Taledynamic.Core.Interfaces
{
    public interface ITelegramService
    {
        public Task<GenericCreateResponse<TelegramUserDto>> Authorize(TelegramAuthorizeRequest request, User user);
        public Task<GenericGetResponse<TelegramUserDto>> Get(TelegramGetRequest request, User user );
        public Task<GenericDeleteResponse<TelegramUserDto>> Revoke(TelegramRevokeRequest request);
    }
}