using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Taledynamic.Api.Attributes;
using Taledynamic.Core.Interfaces;

namespace Taledynamic.Api.Controllers
{
    [ApiController]
    [JwtAuthorize]
    [Route("integration/[controller]")]
    public class TelegramController: BaseController
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
        public void CreateData()
        {
            throw new NotImplementedException();
        }
        
        [HttpGet("data/get")]
        public void GetData()
        {
            throw new NotImplementedException();
        }
        
        [HttpPut("data/update")]
        public void UpdateData()
        {
            throw new NotImplementedException();
        }
        
        [HttpDelete("data/delete")]
        public void DeleteData()
        {
            throw new NotImplementedException();
        }
        
    }
}