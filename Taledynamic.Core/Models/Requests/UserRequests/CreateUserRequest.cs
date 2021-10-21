using System.ComponentModel.DataAnnotations;
using System.Text;
using Taledynamic.Core.Models.Internal;

namespace Taledynamic.Core.Models.Requests.UserRequests
{
    public class CreateUserRequest: BaseRequest
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [MinLength(6)]
        public string Password { get; set; }

        [Required]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }

        public override ValidateState IsValid()
        {
            StringBuilder sb = new StringBuilder();

            if (Email == null)
            {
                sb.Append("Email is not set.");
                ;
            }

            if (Password == null || ConfirmPassword == null)
            {
                sb.Append("Password is not set.");
            }

            if (sb.Length != 0)
            {
                return new ValidateState(false, sb.ToString());
            }

            return new ValidateState(true, "Success");
        }
    }
}