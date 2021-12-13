using System.Text;
using Taledynamic.DAL.Models.Internal;

namespace Taledynamic.DAL.Models.Requests.TableRequests
{
    public class GetTableDataRequest: BaseRequest
    {
        public override ValidateState IsValid()
        {
            return new ValidateState(true, "");
        }
    }
}