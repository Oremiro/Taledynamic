using Taledynamic.DAL.Models.DTOs;

namespace Taledynamic.DAL.Models.Responses.TableResponses
{
    public class GetTableResponse: BaseResponse
    {
        public TableDto Table { get; set; }
    }
}