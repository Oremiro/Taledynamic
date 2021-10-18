using System.ComponentModel.DataAnnotations;

namespace Taledynamic.Core.Models.Requests.UserRequests
{
    public class GetUserRequest: BaseRequest
    {
        [Required]
        public int Id { get; set; }   
    }
}