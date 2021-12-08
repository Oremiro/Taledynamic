using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Taledynamic.Api.Attributes;

namespace Taledynamic.Api.Controllers
{
    [ApiController]
    [JwtAuthorize]
    [Route("integration/[controller]")]
    public class TelegramController: BaseController
    {
        public TelegramController()
        {
            
        }

        [HttpPost("register")]
        public Task RegisterTgUser()
        {
            throw new NotImplementedException();
        }
    }
}