using System.ComponentModel.DataAnnotations;

namespace Taledynamic.Core.Models.Requests.UserRequests
{
    public class GetActiveUserByEmailRequest: BaseRequest
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } 
    }
}