using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Taledynamic.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("auth")]
    public class UserController: ControllerBase
    {
      
    }
}