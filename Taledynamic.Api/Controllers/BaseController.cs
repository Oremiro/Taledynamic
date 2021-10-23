using Microsoft.AspNetCore.Mvc;
using Taledynamic.DAL.Entities;

namespace Taledynamic.Api.Controllers
{
    [Controller]
    public abstract class BaseController: ControllerBase
    {
        public User CustomUser => (User) HttpContext.Items["User"];
    }
}