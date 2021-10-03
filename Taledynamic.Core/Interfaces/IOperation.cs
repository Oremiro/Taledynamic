using System.Threading.Tasks;

namespace Taledynamic.Core.Interfaces
{
    public interface IOperation<TRequest, TResponse> 
        where TResponse: class
        where TRequest: class
    {
        public Task<TResponse> ExecuteAsync(TRequest request);
    }
}