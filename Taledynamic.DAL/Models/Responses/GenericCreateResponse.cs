namespace Taledynamic.DAL.Models.Responses
{
    public class GenericCreateResponse<T>: BaseResponse
    {
        public T Item { get; set; }
    }
}