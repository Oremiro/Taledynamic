using Taledynamic.Core.Models.DTOs;

namespace Taledynamic.Core.Models.Responses.UserResponses
{
    public class GetUserResponse: BaseResponse
    {
        public UserDto User { get; set; }   
    }
}