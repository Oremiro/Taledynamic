using System.Collections.Generic;
using Taledynamic.DAL.Models.DTOs;

namespace Taledynamic.DAL.Models.Responses.UserResponses
{
    public class GetUsersResponse: BaseResponse
    {
        public List<UserDto> Users { get; set; }  
    }
}