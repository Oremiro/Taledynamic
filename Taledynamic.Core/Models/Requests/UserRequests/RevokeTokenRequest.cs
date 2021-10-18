namespace Taledynamic.Core.Models.Requests.UserRequests
{
    public class RevokeTokenRequest: BaseRequest
    {
        public string RefreshToken { get; set; }
    }
}