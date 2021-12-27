using System;
using System.Collections.Generic;
//using Taledynamic.DAL.Entities;

namespace Taledynamic.Sheets
{
    
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class JwtAuthorizeAttribute: Attribute, IAuthorizationFilter
    {
        public JwtAuthorizeAttribute()
        {
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var user = (User)context.HttpContext.Items["User"];
            if (user == null)
            {
                context.Result = new JsonResult(new { message = "Unauthorized" }) { StatusCode = StatusCodes.Status401Unauthorized };
            }
        }      
    }
}