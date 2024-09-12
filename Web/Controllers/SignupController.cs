using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    public class SignupController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
