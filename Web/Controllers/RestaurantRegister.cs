using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    public class RestaurantRegister : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
