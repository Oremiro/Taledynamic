using System.Net;

namespace Taledynamic.Core.Exceptions
{
    public class BadRequestException : BaseHttpException
    {
        private const HttpStatusCode Code = HttpStatusCode.BadRequest;

        public BadRequestException(string message) : base(message, Code)
        {
        }
    }
}