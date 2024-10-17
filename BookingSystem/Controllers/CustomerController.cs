using Data.Constants;
using Data.Models;
using Infra.Helpers;
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
            return View(new tbCustomer());
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

        public async Task<string> CreateCustomer(tbCustomer customer)
        {
            tbCustomer result = await CustomerApiRH.CreateCustomer(customer);
            if(result.Id != 0)
            {
                return ResponseStatus.Success;
            }
            else
            {
                return ResponseStatus.Fail;
            }

           

        }

    }
}
