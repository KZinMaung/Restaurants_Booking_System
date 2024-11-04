using Data.Constants;
using Data.Dtos;
using Data.Models;
using Data.ViewModels;
using Infra.Helpers;
using Microsoft.AspNetCore.Authorization;
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
            
            var result = User.Identity.IsAuthenticated.ToString();
            int cusId = 0;

            if (result == "True")
            {
                cusId = int.Parse(User.Claims.ToArray()[AuthDataIndex.Id].Value);
            }
            tbCustomer cus = await CustomerApiRH.GetById(cusId);

            ReserveViewModel viewModel = new ReserveViewModel();
            viewModel.Restaurant = res;
            viewModel.Customer = cus;

            return View(viewModel);
        }

        

        public IActionResult LoadMenu()
        {
            return PartialView("_Menu");
        }

       

        public IActionResult LoadReviews()
        {
            return PartialView("_Reviews");
        }

        [Authorize]
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

        public async Task<IActionResult> CustomerForm(string FormType, int Id)
        {

            tbCustomer customer = new tbCustomer();
            if (FormType == "Add")
            {
                return PartialView("_customerForm", customer);
            }

            else
            {
                tbCustomer result = await CustomerApiRH.GetById(Id);
                return PartialView("_customerForm", result);

            }
        }


        public async Task<IActionResult> UpSert(tbCustomer customer)
        {
            
            ResponseData result = await CustomerApiRH.UpSert(customer);
            return Json(result);
        }

    }
}
