using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    public class ViewAllRestaurantsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
