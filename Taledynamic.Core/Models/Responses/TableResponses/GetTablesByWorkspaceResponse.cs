using System.Collections.Generic;
using Taledynamic.Core.Entities;

namespace Taledynamic.Core.Models.Responses.TableResponses
{
    public class GetTablesByWorkspaceResponse: BaseResponse
    {
        public List<Table> Tables { get; set; }
    }
}