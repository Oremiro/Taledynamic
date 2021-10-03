using System.ComponentModel.DataAnnotations;

namespace Taledynamic.Core.Models.Requests
{
    public class AuthenticateRequest: BaseRequest
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}