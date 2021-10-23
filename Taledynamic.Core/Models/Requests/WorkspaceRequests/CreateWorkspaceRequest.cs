using System.ComponentModel.DataAnnotations;
using System.Text;
using Taledynamic.Core.Models.Internal;

namespace Taledynamic.Core.Models.Requests.WorkspaceRequests
{
    public class CreateWorkspaceRequest: BaseRequest
    {
        [Required]
        public string Name { get; set;}
        public override ValidateState IsValid()
        {
            StringBuilder sb = new StringBuilder();

            if (Name == null)
            {
                sb.AppendLine("Name for workspace is not set.");
            }

            if (sb.Length != 0)
            {
                return new ValidateState(false, sb.ToString());
            }

            return new ValidateState(true, "Success.");
        }
    }
}