using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Taledynamic.Core.Interfaces
{
    public interface IOperation<TRequest, TResponse> 
        where TResponse: class
        where TRequest: class
    {
        public Task<TResponse> ExecuteAsync(TaledynamicContext context, TRequest request);
    }
}