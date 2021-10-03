using System.ComponentModel.DataAnnotations;

namespace Taledynamic.Core.Models.Requests
{
    public class AuthenticateRequest: BaseRequest
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }
    }
}