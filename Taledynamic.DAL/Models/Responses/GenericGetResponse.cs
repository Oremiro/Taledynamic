namespace Taledynamic.DAL.Models.Responses
{
    public class GenericGetResponse<T>: BaseResponse
    {
        public T Item { get; set; }
    }
}