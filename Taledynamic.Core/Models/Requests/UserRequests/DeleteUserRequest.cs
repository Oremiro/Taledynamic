using System.ComponentModel.DataAnnotations;

namespace Taledynamic.Core.Models.Requests.UserRequests
{
    public class DeleteUserRequest: BaseRequest
    {
        [Required]
        public int UserId { get; set; }
    }
}