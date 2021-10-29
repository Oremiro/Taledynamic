using System;
using System.Net;

namespace Taledynamic.Core.Exceptions
{
    public abstract class BaseHttpException: Exception
    {
        public override string Message { get; }
        public HttpStatusCode HttpStatusCode { get; }

        protected BaseHttpException(string message, HttpStatusCode code)
        {
            Message = message;
            HttpStatusCode = code;
        }
    }
}