using CompanyOrderManagement.Domain.Exceptions;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Net.Mime;
using Newtonsoft.Json;
using CompanyOrderManagement.Application.Logging.Services.Interfaces;
namespace CompanyOrderManagement.WebAPI.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILoggerService _logger;

        public ExceptionMiddleware(RequestDelegate next, ILoggerService logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception exception)
            {
                _logger.LogException(exception, "An exception occurred.");

                await HandleExceptionAsync(context, exception);
            }
        }
        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = MediaTypeNames.Application.Json;

            if (exception.GetType() == typeof(BusinessException)) return CreateBusinessException(context, exception);

            return CreateInternalException(context, exception);
        }

        private Task CreateBusinessException(HttpContext context, Exception exception)
        {
            context.Response.StatusCode = Convert.ToInt32(HttpStatusCode.BadRequest);

            var problemDetails = new BusinessProblemDetails
            {
                Status = StatusCodes.Status400BadRequest,
                Type = "https://example.com/probs/business",
                Title = "Business exception",
                Detail = exception.Message,
                Instance = context.TraceIdentifier
            };

            return context.Response.WriteAsync(JsonConvert.SerializeObject(problemDetails));
        }

        private Task CreateInternalException(HttpContext context, Exception exception)
        {
            context.Response.StatusCode = Convert.ToInt32(HttpStatusCode.InternalServerError);

            var problemDetails = new ProblemDetails
            {
                Status = StatusCodes.Status500InternalServerError,
                Type = "https://example.com/probs/internal",
                Title = "Internal exception",
                Detail = exception.Message, 
                Instance = context.TraceIdentifier
            };

            return context.Response.WriteAsync(JsonConvert.SerializeObject(problemDetails));
        }
    }
}
