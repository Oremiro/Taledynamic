using Taledynamic.Core.Models.Internal;

namespace Taledynamic.Core.Models.Requests
{
    public abstract class BaseRequest
    {
        public abstract ValidateState IsValid();
    }
}