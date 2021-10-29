using System;
using System.Net;

namespace Taledynamic.Core.Exceptions
{
    public sealed class NotFoundException: BaseHttpException
    {
        private const HttpStatusCode Code = HttpStatusCode.NotFound;

        public NotFoundException(string message) : base(message, Code)
        {
         
        }
    }
}