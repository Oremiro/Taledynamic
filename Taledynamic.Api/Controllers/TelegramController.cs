using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using Taledynamic.Api.Attributes;
using Taledynamic.Core.Interfaces;
using Taledynamic.DAL.Models.DTOs;
using Taledynamic.DAL.Models.Requests.TelegramRequests;
using Taledynamic.DAL.Models.Responses;
using Taledynamic.DAL.Models.Responses.UserResponses;

namespace Taledynamic.Api.Controllers
{
    [ApiController]
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

        [AllowAnonymous]
        [HttpPost("tg-authorize")]
        public async Task<AuthenticateResponse> TgTokenAuthorize([FromBody] TelegramAuthorizeRequest request)
        {
            Log.Information($"[{nameof(TableController)}]: Method 'CreateData' started.");
            var response = await _telegramService.TgAuthorize(request);
            Log.Information($"[{nameof(TableController)}]: Method 'CreateData' started.");
            return response;
        }
        [JwtAuthorize]
        [HttpPost("sync-table")]
        public async Task<OkResult> SyncTable([FromBody] CreateTelegramDataRequest request)
        {
            Log.Information($"[{nameof(TableController)}]: Method 'GetData' started.");
      
            await _telegramService.SyncTable(request, CustomUser);
            Log.Information($"[{nameof(TableController)}]: Method 'GetData' started.");
            return Ok();
        }
        [JwtAuthorize]
        [HttpPost("authorize")]
        public async Task<GenericCreateResponse<TelegramUserDto>> Authorize([FromBody] TelegramAuthorizeRequest request)
        {
            Log.Information($"[{nameof(TableController)}]: Method 'CreateData' started.");
            var response = await _telegramService.Authorize(request, CustomUser);
            Log.Information($"[{nameof(TableController)}]: Method 'CreateData' started.");
            return response;
        }

        [HttpGet("get")]
        public async Task<GenericGetResponse<TelegramUserDto>> Get([FromQuery] TelegramGetRequest request)
        {
            Log.Information($"[{nameof(TableController)}]: Method 'CreateData' started.");
            var response = await _telegramService.Get(request, CustomUser);
            Log.Information($"[{nameof(TableController)}]: Method 'CreateData' started.");
            return response;
        }
        [JwtAuthorize]
        [HttpPost("revoke")]
        public async Task<GenericDeleteResponse<TelegramUserDto>> Revoke([FromBody] TelegramRevokeRequest request)
        {
            Log.Information($"[{nameof(TableController)}]: Method 'CreateData' started.");
            var response = await _telegramService.Revoke(request);
            Log.Information($"[{nameof(TableController)}]: Method 'CreateData' started.");
            return response;
        }
        [JwtAuthorize]
        [HttpPost("data/create")]
        public async Task<GenericCreateResponse<TelegramDataDto>> CreateData([FromBody] CreateTelegramDataRequest request)
        {
            Log.Information($"[{nameof(TableController)}]: Method 'CreateData' started.");
            var response = await _telegramDataService.CreateTelegramDataAsync(request, CustomUser);
            Log.Information($"[{nameof(TableController)}]: Method 'CreateData' started.");
            return response;
        }
        [JwtAuthorize]
        [HttpGet("data/get")]
        public async Task<GenericGetResponse<TelegramDataDto>> GetData([FromQuery] GetTelegramDataRequest request)
        {
            Log.Information($"[{nameof(TableController)}]: Method 'GetData' started.");
            var response = await _telegramDataService.ReadTelegramDataAsync(request, CustomUser);
            Log.Information($"[{nameof(TableController)}]: Method 'GetData' started.");
            return response;
        }
        [JwtAuthorize]
        [HttpPut("data/update")]
        public async Task<EmptyUpdateResponse> UpdateData([FromBody] UpdateTelegramDataRequest request)
        {
            Log.Information($"[{nameof(TableController)}]: Method 'UpdateData' started.");
            var response = await _telegramDataService.UpdateTelegramDataAsync(request, CustomUser);
            Log.Information($"[{nameof(TableController)}]: Method 'UpdateData' started.");
            return response;
        }
        [JwtAuthorize]
        [HttpDelete("data/delete")]
        public async Task<GenericDeleteResponse<TelegramDataDto>> DeleteData([FromBody] DeleteTelegramDataRequest request)
        {
            Log.Information($"[{nameof(TableController)}]: Method 'DeleteData' started.");
            var response = await _telegramDataService.DeleteTelegramDataAsync(request, CustomUser);
            Log.Information($"[{nameof(TableController)}]: Method 'DeleteData' started.");
            return response;
        }
    }
}