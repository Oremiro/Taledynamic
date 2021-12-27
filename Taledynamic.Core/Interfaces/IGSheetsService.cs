using System.Threading.Tasks;
using Taledynamic.DAL.Entities;
using Taledynamic.DAL.Models.DTOs;
using Taledynamic.DAL.Models.Requests.TelegramRequests;
using Taledynamic.DAL.Models.Responses;

namespace Taledynamic.Core.Interfaces
{
    public interface IGSheetsService
    {
        //public Task<GenericCreateResponse<GSheetsDto>> Export(TelegramAuthorizeRequest request, User user);
        public Task<EmptyUpdateResponse> Export(UpdateTelegramDataRequest request);
    }
}