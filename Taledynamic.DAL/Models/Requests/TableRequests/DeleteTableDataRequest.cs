using Taledynamic.DAL.Models.Internal;

namespace Taledynamic.DAL.Models.Requests.TableRequests
{
    public class DeleteTableDataRequest: BaseRequest
    {
        public string UId { get; set; }
        public override ValidateState IsValid()
        {
            throw new System.NotImplementedException();
        }
    }
}