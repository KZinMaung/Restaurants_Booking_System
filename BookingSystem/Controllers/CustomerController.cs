using Microsoft.AspNetCore.Mvc;

namespace BookingSystem.Controllers
{
    public class CustomerController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Register()
        {
            return View();
        }

        public IActionResult Reserve()
        {
            return View();
        }


        public IActionResult LoadMenu()
        {
            return PartialView("_Menu");
        }

        public IActionResult LoadAbout()
        {
            return PartialView("_About");
        }

        public IActionResult LoadReviews()
        {
            return PartialView("_Reviews");
        }

        public IActionResult Bookings()
        {
            return View();
        }



    }
}
