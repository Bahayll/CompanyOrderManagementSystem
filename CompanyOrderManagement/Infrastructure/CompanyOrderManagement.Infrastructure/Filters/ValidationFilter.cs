using CompanyOrderManagement.Application.Logging.Services.Interfaces;
using CompanyOrderManagement.Application.ResponseModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompanyOrderManagement.Infrastructure.Filters
{
    public class ValidationFilter : IActionFilter
    {
        private readonly ILoggerService _logger;

        public ValidationFilter(ILoggerService logger)
        {
            _logger = logger;
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            var errors = context.ModelState
            .Where(v => v.Value.Errors.Count > 0)
            .Select(v => new ValidationError(v.Key, v.Value.Errors.First().ErrorMessage))
            .ToList();

            if (errors.Any())
            {
                var errorMessages = string.Join("; ", errors.Select(e => $"{e.Property}: {e.Message}"));
                _logger.Error($"Validation errors occurred: {errorMessages}");


                var response = ApiResponse<NoContentResponse>.ValidationError(errors);
                context.Result = new BadRequestObjectResult(response);
            }

        }
    }
}
