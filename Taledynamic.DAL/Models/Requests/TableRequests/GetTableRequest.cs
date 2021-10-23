using System.ComponentModel.DataAnnotations;
using System.Text;
using Taledynamic.DAL.Models.Internal;

namespace Taledynamic.DAL.Models.Requests.TableRequests
{
    public class GetTableRequest: BaseRequest
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public int WorkspaceId { get; set; }
        public override ValidateState IsValid()
        {
            var sb = new StringBuilder();
            if (Id == default)
            {
                sb.Append("Id contains default value");
            }
            if (WorkspaceId == default)
            {
                sb.Append("WorkspaceId contains default value");
            }

            if (sb.Length != 0)
            {
                return new ValidateState(false, sb.ToString());
            }

            return new ValidateState(true, "Success");
        }
    }
}