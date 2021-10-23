using System.ComponentModel.DataAnnotations;
using System.Text;
using Taledynamic.Core.Models.Internal;

namespace Taledynamic.Core.Models.Requests.TableRequests
{
    public class GetTablesByWorkspaceRequest: BaseRequest
    {
        [Required]
        public int WorkspaceId { get; set; }

        public override ValidateState IsValid()
        {
            var sb = new StringBuilder();
            if (WorkspaceId == default)
            {
                sb.AppendLine("WorkspaceId contains default value");
            }

            if (sb.Length != 0)
            {
                return new ValidateState(false, sb.ToString());
            }

            return new ValidateState(true, "Success");
        }
    }
}