using System.ComponentModel.DataAnnotations;
using System.Text;
using Taledynamic.DAL.Models.Internal;

namespace Taledynamic.DAL.Models.Requests.WorkspaceRequests
{
    public class UpdateWorkspaceRequest: BaseRequest
    {
        [Required]
        public int Id { get; set; }
        public string Name { get; set;}
        
        public override ValidateState IsValid()
        {
            StringBuilder sb = new StringBuilder();
            
            if (Id == default)
            {
                sb.Append("Id is default.\n");
            }

            if (Name == null)
            {
                sb.Append("Name for project is not set.");
            }

            if (sb.Length != 0)
            {
                return new ValidateState(false, sb.ToString());
            }

            return new ValidateState(true, "Success.");
        }
    }
}