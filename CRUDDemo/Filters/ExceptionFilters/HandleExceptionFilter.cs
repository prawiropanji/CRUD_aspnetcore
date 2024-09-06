using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CRUDDemo.Filters.ExceptionFilters
{
    public class HandleExceptionFilter : IExceptionFilter
    {
        private readonly ILogger<HandleExceptionFilter> _logger;
        private readonly IWebHostEnvironment _webHost;

        public HandleExceptionFilter(ILogger<HandleExceptionFilter> logger, IWebHostEnvironment webHost)
        {
            _logger = logger;
            _webHost = webHost;
        }
        public void OnException(ExceptionContext context)
        {
            _logger.LogError("Error {ErrorMessage}\n {ErrorType}", 
                context.Exception.Message, 
                context.Exception.GetType().ToString());

            if (_webHost.IsDevelopment())
            {
                context.Result = new ContentResult() { Content= $"<h1>Error 500</h1><p>{context.Exception.Message}</p>", ContentType="text/html" };
                return;
            }

        }
    }
}
