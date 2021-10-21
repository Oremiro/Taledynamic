using System.Net;

namespace Taledynamic.Core.Exceptions
{
    public sealed class InternalServerErrorException : BaseHttpException
    {
        private const HttpStatusCode Code = HttpStatusCode.InternalServerError;

        public InternalServerErrorException(string message) : base(message, Code)
        {
        }
    }
}