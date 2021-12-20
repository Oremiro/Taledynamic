using System.Threading.Tasks;
using Taledynamic.DAL.Models.DTOs;
using Taledynamic.DAL.Models.Requests.TelegramRequests;
using Taledynamic.DAL.Models.Responses;

namespace Taledynamic.Core.Interfaces
{
    public interface ITelegramDataService
    {
        public Task<EmptyUpdateResponse> UpdateTelegramDataAsync(UpdateTelegramDataRequest request);
        public Task<GenericCreateResponse<TelegramDataDto>> CreateTelegramDataAsync(CreateTelegramDataRequest request);
        public Task<GenericDeleteResponse<TelegramDataDto>> DeleteTelegramDataAsync(DeleteTelegramDataRequest request);
        public Task<GenericGetResponse<TelegramDataDto>> ReadTelegramDataAsync(GetTelegramDataRequest request);
    }
}