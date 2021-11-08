using Taledynamic.DAL.Models.DTOs;

namespace Taledynamic.DAL.Models.Responses.TableResponses
{
    public class CreateTableResponse: BaseResponse
    {
        public TableDto Table { get; set; }
    }
}