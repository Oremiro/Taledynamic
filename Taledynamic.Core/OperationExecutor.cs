using System;
using System.Diagnostics;
using System.Net;
using System.Threading.Tasks;
using Taledynamic.Core.Interfaces;
using Taledynamic.Core.Models.Responses;

namespace Taledynamic.Core
{
    public class OperationExecutor<TRequest, TResponse> 
        where TRequest: class
        where TResponse: BaseResponse, new()
    {
        public IOperation<TRequest, TResponse> Operation { get; set; }

        public OperationExecutor(IOperation<TRequest, TResponse> operation)
        {
            Operation = operation;
        }

        public async Task<TResponse> ExecuteAsync(TRequest request)
        {
            try
            {
                var stopWatch = Stopwatch.StartNew();
                stopWatch.Start();
                var response = await Operation.ExecuteAsync(request);
                stopWatch.Stop();
                return response;
            }
            catch (Exception exception)
            {
                var message = $"There are some errors during operation: {exception.StackTrace}";
                var statusCode = HttpStatusCode.BadRequest;
                return new TResponse()
                {
                    Message = message, StatusCode = statusCode
                };
            }
        }
        
    }
}