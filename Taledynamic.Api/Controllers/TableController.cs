using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using Taledynamic.Api.Attributes;
using Taledynamic.Core.Interfaces;
using Taledynamic.DAL.Entities;
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
            Log.Information($"[{nameof(TableController)}]: Method '{MethodBase.GetCurrentMethod()?.Name}' started.");
            var response = await _tableService.GetTablesByWorkspaceAsync(request);
            Log.Information($"[{nameof(TableController)}]: Method '{MethodBase.GetCurrentMethod()?.Name}' ended.");
            return response;
        }
        
        [HttpGet("get")]
        public async Task<GetTableResponse> Get([FromQuery] GetTableRequest request)
        {
            Log.Information($"[{nameof(TableController)}]: Method '{MethodBase.GetCurrentMethod()?.Name}' started.");
            var response = await _tableService.GetTableAsync(request);
            Log.Information($"[{nameof(TableController)}]: Method '{MethodBase.GetCurrentMethod()?.Name}' ended.");
            return response;
        }
        
        [HttpPost("create")]
        public async Task<CreateTableResponse> Create([FromBody] CreateTableRequest request)
        {
            Log.Information($"[{nameof(TableController)}]: Method '{MethodBase.GetCurrentMethod()?.Name}' started.");
            var response = await _tableService.CreateTableAsync(request);
            Log.Information($"[{nameof(TableController)}]: Method '{MethodBase.GetCurrentMethod()?.Name}' ended.");
            return response;
        }
        [HttpPut("update")]
        public async Task<UpdateTableResponse> Update([FromBody] UpdateTableRequest request)
        {
            Log.Information($"[{nameof(TableController)}]: Method '{MethodBase.GetCurrentMethod()?.Name}' started.");
            var response = await _tableService.UpdateTableAsync(request);
            Log.Information($"[{nameof(TableController)}]: Method '{MethodBase.GetCurrentMethod()?.Name}' ended.");
            return response;
        }
        [HttpDelete("delete")]
        public async Task<DeleteTableResponse> Delete([FromQuery] DeleteTableRequest request)
        {
            Log.Information($"[{nameof(TableController)}]: Method '{MethodBase.GetCurrentMethod()?.Name}' started.");
            var response = await _tableService.DeleteTableAsync(request);
            Log.Information($"[{nameof(TableController)}]: Method '{MethodBase.GetCurrentMethod()?.Name}' ended.");
            return response;
        }
    }
}