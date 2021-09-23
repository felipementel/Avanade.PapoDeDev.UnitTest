using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Threading.Tasks;

namespace Avanade.PapoDeDev.UnitTest.API.FilterType
{
    public class ExceptionFilterAttribute : Microsoft.AspNetCore.Mvc.Filters.ExceptionFilterAttribute
    {
        public readonly ILogger<ExceptionFilterAttribute> _logger;

        public ExceptionFilterAttribute(ILogger<ExceptionFilterAttribute> logger)
        {
            _logger = logger;
        }

        public override Task OnExceptionAsync(ExceptionContext context)
        {
            var ex = context.Exception;

            _logger.LogError(ex, ex.Message);

            context.Result = new ContentResult
            {
                Content = ex.Message,
                ContentType = "application/json",
                StatusCode = (int)HttpStatusCode.InternalServerError
            };

            context.ExceptionHandled = true;

            return base.OnExceptionAsync(context);
        }
    }
}