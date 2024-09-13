using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    public class RestaurantBookingsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
