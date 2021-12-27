using System.Threading.Tasks;
using Taledynamic.DAL.Entities;
using Taledynamic.DAL.Models.DTOs;
using Taledynamic.DAL.Models.Requests.TelegramRequests;
using Taledynamic.DAL.Models.Responses;

namespace Taledynamic.Core.Interfaces
{
    public interface IGSheetsDataService
    {
        public Task<EmptyUpdateResponse> ExportTableDataAsync(UpdateTelegramDataRequest request, User user);
        public Task<GenericCreateResponse<TelegramDataDto>> CreateTelegramDataAsync(CreateTelegramDataRequest request, User user);
        public Task<GenericDeleteResponse<TelegramDataDto>> DeleteTelegramDataAsync(DeleteTelegramDataRequest request, User user);
        public Task<GenericGetResponse<TelegramDataDto>> ReadTelegramDataAsync(GetTelegramDataRequest request, User user);
    }
}