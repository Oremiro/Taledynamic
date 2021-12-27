using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using Taledynamic.Api.Attributes;
using Taledynamic.Core.Interfaces;
using Taledynamic.DAL.Models.DTOs;
using Taledynamic.DAL.Models.Requests.TelegramRequests; //
using Taledynamic.DAL.Models.Responses;

namespace Taledynamic.Api.Controllers
{
    [ApiController]
    [JwtAuthorize]
    [Route("integration/[controller]")]
    public class GSheetsController : BaseController
    {
        private IGSheetsService _gsheetsService { get; }
        private IGSheetsDataService _gsheetsDataService { get; }

        public GSheetsController(IGSheetsService gsheetsService, IGSheetsDataService gsheetsDataService)
        {
            _gsheetsService = gsheetsService;
            _gsheetsDataService = gsheetsDataService;
        }
         
        [HttpPut("data/update")]
        public async Task<EmptyUpdateResponse> Export([FromBody] UpdateTelegramDataRequest request)
        {
            //String spreadsheetId= "")



            Log.Information($"[{nameof(TableController)}]: Method 'UpdateData' started.");
            var response = await _gsheetsDataService.ExportTableDataAsync(request, CustomUser);
            Log.Information($"[{nameof(TableController)}]: Method 'UpdateData' started.");
            return response;
        }
    }
}
