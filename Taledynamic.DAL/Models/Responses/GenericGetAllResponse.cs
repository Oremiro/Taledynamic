using System.Collections.Generic;

namespace Taledynamic.DAL.Models.Responses
{
    public class GenericGetAllResponse<T>: BaseResponse
    {
        public List<T> Items { get; set; }
    }
}