using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace AwiaEgyptTravel.Web.Filters
{
    public class GlobalExceptionFilter : IExceptionFilter
    {
        private readonly ILogger<GlobalExceptionFilter> _logger;
        private readonly IHostEnvironment _environment;

        public GlobalExceptionFilter(
            ILogger<GlobalExceptionFilter> logger,
            IHostEnvironment environment)
        {
            _logger = logger;
            _environment = environment;
        }

        public void OnException(ExceptionContext context)
        {
            _logger.LogError(context.Exception, context.Exception.Message);

            var result = new ViewResult { ViewName = "Error" };
            
            if (_environment.IsDevelopment())
            {
                result.ViewData["Exception"] = context.Exception;
            }

            context.Result = result;
        }
    }
}
