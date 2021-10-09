using System.Net;

namespace Taledynamic.Core.Models.Responses
{
    public class BaseResponse
    {
        public HttpStatusCode StatusCode { get; set; }
        public string Message { get; set; }
    }
}