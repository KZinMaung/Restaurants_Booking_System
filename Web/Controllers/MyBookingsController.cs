using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    public class MyBookingsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
