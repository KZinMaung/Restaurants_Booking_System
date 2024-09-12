using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    public class RestaurantHome : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
