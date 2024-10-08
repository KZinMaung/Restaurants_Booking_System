using Microsoft.AspNetCore.Mvc;

namespace BookingSystem.Controllers
{
    public class AuthenticationController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
