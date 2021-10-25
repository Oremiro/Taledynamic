using Taledynamic.DAL.Models.Internal;

namespace Taledynamic.DAL.Models.Requests.WorkspaceRequests
{
    public class GetWorkspacesByUserRequest: BaseRequest
    {
        public override ValidateState IsValid()
        {
            return new ValidateState(true, "Success.");
        }
    }
}