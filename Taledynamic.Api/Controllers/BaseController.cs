using Microsoft.AspNetCore.Mvc;
using Taledynamic.Core.Entities;

namespace Taledynamic.Api.Controllers
{
    [Controller]
    public abstract class BaseController: ControllerBase
    {
        public User CustomUser => (User) HttpContext.Items["User"];
    }
}