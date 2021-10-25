using Taledynamic.DAL.Models.Internal;

namespace Taledynamic.DAL.Models.Requests.UserRequests
{
    public class RevokeTokenRequest: BaseRequest
    {
        public string RefreshToken { get; set; }
        public override ValidateState IsValid()
        {
            throw new System.NotImplementedException();
        }
    }
}