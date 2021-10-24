using System.Collections.Generic;
using Taledynamic.DAL.Models.DTOs;

namespace Taledynamic.DAL.Models.Responses.TableResponses
{
    public class GetTablesByWorkspaceResponse: BaseResponse
    {
        public List<TableDto> Tables { get; set; }
    }
}