using Taledynamic.Core.Models.Internal;

namespace Taledynamic.Core.Models.Requests.UserRequests
{
    public class GetUsersRequest: BaseRequest
    {
        public override ValidateState IsValid()
        {
            return new ValidateState(true, "Success");
        }
    }
}