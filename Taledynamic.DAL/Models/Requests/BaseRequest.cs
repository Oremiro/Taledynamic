using Taledynamic.DAL.Models.Internal;

namespace Taledynamic.DAL.Models.Requests
{
    public abstract class BaseRequest
    {
        public abstract ValidateState IsValid();
    }
}