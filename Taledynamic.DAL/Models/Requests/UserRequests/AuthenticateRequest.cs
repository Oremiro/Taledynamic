using System.ComponentModel.DataAnnotations;
using System.Text;
using Taledynamic.DAL.Models.Internal;

namespace Taledynamic.DAL.Models.Requests.UserRequests
{
    public class AuthenticateRequest: BaseRequest
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        public override ValidateState IsValid()
        {
            StringBuilder sb = new StringBuilder();

            if (Email == null)
            {
                sb.Append("Email is not set.");
            }

            if (Password == null)
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