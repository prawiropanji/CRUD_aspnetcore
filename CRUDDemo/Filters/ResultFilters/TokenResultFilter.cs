using Microsoft.AspNetCore.Mvc.Filters;

namespace CRUDDemo.Filters.ResultFilters
{
    public class TokenResultFilter : IResultFilter
    {
        public void OnResultExecuted(ResultExecutedContext context)
        {
            //context.HttpContext.Response.Cookies.Append("Auth-Key", "h-100");
        }

        public void OnResultExecuting(ResultExecutingContext context)
        {

            context.HttpContext.Response.Cookies.Append("Auth-Key", "h-100");

        }
    }
}
