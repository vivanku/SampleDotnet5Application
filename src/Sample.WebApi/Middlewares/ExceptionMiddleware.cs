using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;

namespace Sample.WebApi.Middlewares
{
    /// <summary>
    /// Exception Middleware
    /// </summary>
    public class ExceptionMiddleware
    {

        private readonly RequestDelegate _next;
        private readonly ILogger _logger;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="next"></param>
        public ExceptionMiddleware(RequestDelegate next, ILoggerFactory loggerFactory)
        {
            this._next = next;
            _logger = loggerFactory.CreateLogger<ExceptionMiddleware>();

        }

        /// <summary>
        /// Exception Middleware implementation
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            _logger.LogTrace("Inside ExceptionMiddleware");
            var code = HttpStatusCode.InternalServerError; 
            var message = $"Something went wrong. Please contact administrator.";


            var baseException = ex.GetBaseException();
            var exToHandle = baseException ?? ex;

            if (exToHandle is UnauthorizedAccessException)
            {
                code = HttpStatusCode.Forbidden;
            }
            if (exToHandle is ValidationException)
            {
                code = HttpStatusCode.BadRequest;
                message = exToHandle.Message;
            }
            else if (exToHandle is ArgumentException || exToHandle is ArgumentNullException || exToHandle is ArgumentOutOfRangeException)
            {
                code = HttpStatusCode.BadRequest;
                message = $"Arguments are invalid. {exToHandle.Message}";
            }
            else if (exToHandle is InvalidOperationException)
            {
                code = HttpStatusCode.BadRequest;
                message = $"Operation is not allowed for the given arguments. {exToHandle.Message}";
            }
            else if (exToHandle is Exception)
            {
                code = HttpStatusCode.InternalServerError;
                message = $"{message}. Exception Message :  {exToHandle.Message}";
            }
            _logger.LogError("Exception occured : ", message, ex);
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)code;
            return context.Response.WriteAsync(JsonSerializer.Serialize(message));
        }
    }
}
