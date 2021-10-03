using System.ComponentModel.DataAnnotations;

namespace Taledynamic.Core.Models.Requests.UserRequests
{
    public class UpdateUserRequest: BaseRequest
    {
        [EmailAddress]
        public string Email { get; set; }

        [MinLength(6)]
        public string Password { get; set; }

        [Compare("Password")]
        public string ConfirmPassword { get; set; }
    }
}