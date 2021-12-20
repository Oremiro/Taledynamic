using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using Taledynamic.Api.Attributes;
using Taledynamic.Core.Interfaces;
using Taledynamic.DAL.Models.DTOs;
using Taledynamic.DAL.Models.Requests.TelegramRequests;
using Taledynamic.DAL.Models.Responses;

namespace Taledynamic.Api.Controllers
{
    [ApiController]
    [JwtAuthorize]
    [Route("integration/[controller]")]
    public class TelegramController : BaseController
    {
        private ITelegramService _telegramService { get; set; }
        private ITelegramDataService _telegramDataService { get; set; }

        public TelegramController(ITelegramService telegramService, ITelegramDataService telegramDataService)
        {
            _telegramService = telegramService;
            _telegramDataService = telegramDataService;
        }

        [HttpPost("authorize")]
        public async Task Authorize()
        {
            throw new NotImplementedException();
        }

        [HttpGet("get")]
        public async Task Get()
        {
            throw new NotImplementedException();
        }

        [HttpPost("revoke")]
        public async Task Revoke()
        {
            throw new NotImplementedException();
        }

        [HttpPost("data/create")]
        public async Task<GenericCreateResponse<TelegramDataDto>> CreateData([FromBody] CreateTelegramDataRequest request)
        {
            Log.Information($"[{nameof(TableController)}]: Method 'CreateData' started.");
            var response = await _telegramDataService.CreateTelegramDataAsync(request);
            Log.Information($"[{nameof(TableController)}]: Method 'CreateData' started.");
            return response;
        }

        [HttpGet("data/get")]
        public async Task<GenericGetResponse<TelegramDataDto>> GetData([FromQuery] GetTelegramDataRequest request)
        {
            Log.Information($"[{nameof(TableController)}]: Method 'GetData' started.");
            var response = await _telegramDataService.ReadTelegramDataAsync(request);
            Log.Information($"[{nameof(TableController)}]: Method 'GetData' started.");
            return response;
        }

        [HttpPut("data/update")]
        public async Task<EmptyUpdateResponse> UpdateData([FromBody] UpdateTelegramDataRequest request)
        {
            Log.Information($"[{nameof(TableController)}]: Method 'UpdateData' started.");
            var response = await _telegramDataService.UpdateTelegramDataAsync(request);
            Log.Information($"[{nameof(TableController)}]: Method 'UpdateData' started.");
            return response;
        }

        [HttpDelete("data/delete")]
        public async Task<GenericDeleteResponse<TelegramDataDto>> DeleteData([FromBody] DeleteTelegramDataRequest request)
        {
            Log.Information($"[{nameof(TableController)}]: Method 'DeleteData' started.");
            var response = await _telegramDataService.DeleteTelegramDataAsync(request);
            Log.Information($"[{nameof(TableController)}]: Method 'DeleteData' started.");
            return response;
        }
    }
}