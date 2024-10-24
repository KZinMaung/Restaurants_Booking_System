using Data.Constants;
using Data.Models;
using Data.ViewModels;
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

        [HttpGet("Customer/Reserve/{resId}")]
        public async Task<IActionResult> Reserve(int resId)
        {
            tbRestaurant res = await RestaurantApiRH.GetById(resId);
            List<tbRestaurantSchedule> schedules = await RestaurantScheduleApiRH.GetList(resId);

            var result = User.Identity.IsAuthenticated.ToString();
            int cusId = 0;

            if (result == "True")
            {
                cusId = int.Parse(User.Claims.ToArray()[AuthDataIndex.Id].Value);
            }
            tbCustomer cus = await CustomerApiRH.GetById(cusId);

            ReserveViewModel viewModel = new ReserveViewModel();
            viewModel.restaurant = res;
            viewModel.schedules = schedules;
            viewModel.customer = cus;

            return View(viewModel);
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
