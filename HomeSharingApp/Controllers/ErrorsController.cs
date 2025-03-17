using Microsoft.AspNetCore.Mvc;

namespace HomeSharingApp.Controllers
{
    public class ErrorsController : Controller
    {
        public IActionResult PageNotFound(int statusCode)
        {
            return View("404");
        }
    }
}
