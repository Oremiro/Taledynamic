using System.Net;

namespace Taledynamic.DAL.Models.Responses
{
    public class BaseResponse
    {
        public HttpStatusCode StatusCode { get; set; }
        public string Message { get; set; }
    }
}