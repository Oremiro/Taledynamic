using System.ComponentModel.DataAnnotations;
using System.Text;
using Taledynamic.Core.Models.Internal;

namespace Taledynamic.Core.Models.Requests.UserRequests
{
    public class IsEmailUsedRequest: BaseRequest
    {
        [Required]
        public string Email { get; set; }

        public override ValidateState IsValid()
        {
            StringBuilder sb = new StringBuilder();
            if (Email == null)
            {
                sb.Append("UserId is default.");
            }

            if (sb.Length == 0)
            {
                return new ValidateState(false, sb.ToString());
            }

            return new ValidateState(true, "Success");
        }
    }
}