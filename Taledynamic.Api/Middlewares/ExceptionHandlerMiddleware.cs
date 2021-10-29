using System;
using System.Collections.Generic;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Taledynamic.Core.Exceptions;
using Taledynamic.DAL.Models.Responses;

namespace Taledynamic.Api.Middlewares
{
    public class ExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionHandlerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (BaseHttpException httpException)
            {
                var response = context.Response;
                response.ContentType = "application/json";
                response.StatusCode = (int) httpException.HttpStatusCode;

                var result = new BaseResponse
                {
                    Message = httpException.Message ?? "",
                    StatusCode = httpException.HttpStatusCode
                };
                await response.WriteAsJsonAsync(result);
            }
            catch (Exception exception)
            {
                var response = context.Response;
                response.ContentType = "application/json";
                response.StatusCode = (int) HttpStatusCode.InternalServerError;

                var result = new BaseResponse
                {
                    Message = exception.Message ?? "",
                    StatusCode = HttpStatusCode.InternalServerError
                };
                await response.WriteAsJsonAsync(result);
            }
        }
    }
}