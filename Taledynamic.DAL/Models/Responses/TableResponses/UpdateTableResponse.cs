using Taledynamic.DAL.Models.DTOs;

namespace Taledynamic.DAL.Models.Responses.TableResponses
{
    public class UpdateTableResponse: BaseResponse
    {
        public TableDto Table { get; set; }
    }
}