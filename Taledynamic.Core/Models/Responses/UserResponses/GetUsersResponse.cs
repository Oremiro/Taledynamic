using System.Collections.Generic;
using Taledynamic.Core.Models.DTOs;

namespace Taledynamic.Core.Models.Responses.UserResponses
{
    public class GetUsersResponse: BaseResponse
    {
        public List<GetUserDto> Users { get; set; }  
    }
}