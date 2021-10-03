using System.Threading.Tasks;
using Taledynamic.Core.Interfaces;
using Taledynamic.Core.Models.Requests;
using Taledynamic.Core.Models.Responses;

namespace Taledynamic.Core.Operations.User
{
    public class RevokeTokenOperation: IOperation<RevokeTokenRequest, RevokeTokenResponse>
    {
        public Task<RevokeTokenResponse> ExecuteAsync(RevokeTokenRequest request)
        {
            throw new System.NotImplementedException();
        }
    }
}