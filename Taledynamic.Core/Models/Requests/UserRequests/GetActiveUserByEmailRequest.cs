using System.ComponentModel.DataAnnotations;
using System.Text;
using Taledynamic.Core.Models.Internal;

namespace Taledynamic.Core.Models.Requests.UserRequests
{
    public class GetActiveUserByEmailRequest: BaseRequest
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        public override ValidateState IsValid()
        {
            StringBuilder sb = new StringBuilder();

            if (string.IsNullOrEmpty(Email))
            {
                sb.Append("Email is empty.");
            }

            if (sb.Length != 0)
            {
                return new ValidateState(false, sb.ToString());
            }

            return new ValidateState(true, "Success");
        }
    }
}