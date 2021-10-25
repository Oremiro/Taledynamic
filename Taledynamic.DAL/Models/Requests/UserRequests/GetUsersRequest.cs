using Taledynamic.DAL.Models.Internal;

namespace Taledynamic.DAL.Models.Requests.UserRequests
{
    public class GetUsersRequest: BaseRequest
    {
        public override ValidateState IsValid()
        {
            return new ValidateState(true, "Success");
        }
    }
}