using System.Net;

namespace Taledynamic.Core.Models.Responses
{
    public abstract class BaseResponse
    {
        public HttpStatusCode StatusCode { get; set; }
    }
}