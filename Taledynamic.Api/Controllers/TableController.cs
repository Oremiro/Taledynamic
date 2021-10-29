using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Taledynamic.Api.Attributes;
using Taledynamic.Core.Interfaces;
using Taledynamic.DAL.Models.Requests.TableRequests;
using Taledynamic.DAL.Models.Responses.TableResponses;

namespace Taledynamic.Api.Controllers
{
    [ApiController]
    [JwtAuthorize]
    [Route("data/[controller]")]
    public class TableController: BaseController
    {
        private ITableService _tableService { get;  }
        public TableController(ITableService tableService)
        {
            _tableService = tableService;
        }

        [HttpGet("get-filtered-by-workspace")]
        public async Task<GetTablesByWorkspaceResponse> GetFilteredByWorkspace([FromQuery] GetTablesByWorkspaceRequest request)
        {
            var response = await _tableService.GetTablesByWorkspaceAsync(request);
            return response;
        }
        
        [HttpGet("get")]
        public async Task<GetTableResponse> Get([FromQuery] GetTableRequest request)
        {
            var response = await _tableService.GetTableAsync(request);
            return response;
        }
        
        [HttpPost("create")]
        public async Task<CreateTableResponse> Create([FromBody] CreateTableRequest request)
        {
            var response = await _tableService.CreateTableAsync(request);
            return response;
        }
        [HttpPut("update")]
        public async Task<UpdateTableResponse> Update([FromBody] UpdateTableRequest request)
        {
            var response = await _tableService.UpdateTableAsync(request);
            return response;
        }
        [HttpDelete("delete")]
        public async Task<DeleteTableResponse> Delete([FromQuery] DeleteTableRequest request)
        {
            var response = await _tableService.DeleteTableAsync(request);
            return response;
        }
    }
}