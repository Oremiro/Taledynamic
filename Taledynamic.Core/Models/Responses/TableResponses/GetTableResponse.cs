using Taledynamic.Core.Entities;
using Taledynamic.Core.Models.DTOs;

namespace Taledynamic.Core.Models.Responses.TableResponses
{
    public class GetTableResponse: BaseResponse
    {
        public TableDto Table { get; set; }
    }
}