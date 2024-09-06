using Microsoft.AspNetCore.Mvc.Filters;

namespace CRUDDemo.Filters.ActionFilters
{

    public class HeaderActionFilterFactoryAttribute : Attribute, IFilterFactory
    {
        private readonly string _key;
        private readonly string _value;
        private readonly int _order;

        public bool IsReusable => false;

        public HeaderActionFilterFactoryAttribute(string key, string value, int order)
        {
            _key = key;
            _value = value;
            _order = order;
        }

        public IFilterMetadata CreateInstance(IServiceProvider serviceProvider)
        {
            //return filter object
            var filter = serviceProvider.GetRequiredService<HeaderActionFilter>();
            filter.Key = _key;
            filter.Value = _value;
            filter.Order = _order;
            return filter;
        }
    }

    public class HeaderActionFilter : IAsyncActionFilter, IOrderedFilter
    {
        private readonly ILogger<HeaderActionFilter> _logger;
        public string? Key { get; set; }
        public string? Value { get; set; }
        public int Order { get; set; }

        public HeaderActionFilter(ILogger<HeaderActionFilter> logger)
        {
            _logger = logger;
        }


        //public void OnActionExecuted(ActionExecutedContext context)
        //{
        //    _logger.LogInformation("{FilterName}.{MethodName}", nameof(HeaderActionFilter), nameof(OnActionExecuted));
        //}

        //public void OnActionExecuting(ActionExecutingContext context)
        //{
        //    _logger.LogInformation("{FilterName}.{MethodName}", nameof(HeaderActionFilter), nameof(OnActionExecuting));
        //}

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            _logger.LogInformation("{FilterName}.{MethodName} before", nameof(HeaderActionFilter), nameof(OnActionExecutionAsync));
            await next(); // calls the subsequent filter or action method

            if(Key != null) 
                context.HttpContext.Response.Headers[Key] = Value;

            _logger.LogInformation("{FilterName}.{MethodName} after", nameof(HeaderActionFilter), nameof(OnActionExecutionAsync));
        }
    }
}
