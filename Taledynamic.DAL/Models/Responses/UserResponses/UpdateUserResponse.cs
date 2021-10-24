using Taledynamic.DAL.Models.DTOs;

namespace Taledynamic.DAL.Models.Responses.UserResponses
{
    public class UpdateUserResponse: BaseResponse
    {
        public UserDto User { get; set; }
    }
}