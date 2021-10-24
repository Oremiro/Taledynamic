using System.ComponentModel.DataAnnotations;
using System.Text;
using Taledynamic.DAL.Models.Internal;

namespace Taledynamic.DAL.Models.Requests.UserRequests
{
    public class UpdateUserRequest: BaseRequest
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string Password { get; set; }
        
        [EmailAddress]
        public string Email { get; set; }

        [MinLength(6)]
        public string NewPassword { get; set; }

        [Compare("NewPassword")]
        public string ConfirmNewPassword { get; set; }

        public override ValidateState IsValid()
        {
            StringBuilder sb = new StringBuilder();

            if (string.IsNullOrEmpty(
                Password))
            {
                sb.AppendLine("Password is null or empty");
            }
            
            if (Id == default)
            {
                sb.AppendLine("Id is default.");
            }

            if (sb.Length != 0)
            {
                return new ValidateState(false, sb.ToString());
            }

            return new ValidateState(true, "Success");
        }
    }
}