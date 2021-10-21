using System.Net;

namespace Taledynamic.Core.Exceptions
{
    public sealed class UnauthorizedException: BaseHttpException
    { 
        private const HttpStatusCode Code = HttpStatusCode.Unauthorized;

        public UnauthorizedException(string message) : base(message, Code)
        {
         
        }
        
    }
}