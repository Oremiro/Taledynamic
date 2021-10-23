using System.Collections.Generic;
using Taledynamic.Core.Entities;
using Taledynamic.Core.Models.DTOs;

namespace Taledynamic.Core.Models.Responses.TableResponses
{
    public class GetTablesByWorkspaceResponse: BaseResponse
    {
        public List<TableDto> Tables { get; set; }
    }
}