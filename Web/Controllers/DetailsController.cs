using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    public class Details : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
