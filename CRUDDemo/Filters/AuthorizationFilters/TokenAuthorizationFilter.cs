using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CRUDDemo.Filters.AuthorizationFilters
{
    public class TokenAuthorizationFilter : IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            if (!context.HttpContext.Request.Cookies.ContainsKey("Auth-Key"))
            {
                context.Result = new UnauthorizedResult();
                return;
            }
            if (context.HttpContext.Request.Cookies["Auth-Key"] != "h-100")
            {
                context.Result = new UnauthorizedResult();
                return;
            }
        }
    }
}
