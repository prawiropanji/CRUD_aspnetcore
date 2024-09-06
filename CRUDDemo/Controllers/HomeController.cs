using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace CRUDDemo.Controllers
{
    public class HomeController : Controller
    {
        [Route("/[action]")]
        public IActionResult Error()
        {
            var exceptionPathFeature = HttpContext.Features.Get<IExceptionHandlerPathFeature>();
            ViewBag.ErrorMessage = exceptionPathFeature?.Error.Message;

            
            return View();
        }
    }
}
