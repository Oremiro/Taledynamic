using System.Net;

namespace Taledynamic.Core.Exceptions
{
    public class UnauthorizedException: BaseHttpException
    { 
        private const HttpStatusCode Code = HttpStatusCode.Unauthorized;

        public UnauthorizedException(string message) : base(message, Code)
        {
         
        }
        
    }
}