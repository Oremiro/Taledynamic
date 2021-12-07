namespace Taledynamic.DAL.Models.Responses
{
    public class GenericUpdateResponse<T>: BaseResponse
    {
        public T Item { get; set; }
    }
}