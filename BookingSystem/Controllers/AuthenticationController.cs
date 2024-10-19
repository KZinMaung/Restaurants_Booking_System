using BookingSystem.Services;
using Data.Constants;
using Data.Dtos;
using Infra.Helpers;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace BookingSystem.Controllers
{
    public class AuthenticationController : Controller
    {
        IAuthService _authService;
        public AuthenticationController(IAuthService authService)
        {
            this._authService = authService;
        }

        public IActionResult Index()
        {
            return View();
        }


        //public async Task<IActionResult> Login(tbUser user)
        //{
        //    var status = "Fail";
        //    var result = await AuthorizationRequestHelper.AdminLogin(user);
        //    if (result.ID > 0)
        //    {
        //        status = "Success";
        //        _authService.AuthorizeUser(result, HttpContext);
        //    }

        //    return Json(status);
        //}


        [HttpPost]
        public async Task<IActionResult> Login(LoginRequest loginRequest)
        {
            var status = ResponseStatus.Fail;
            try
            {
                AuthenticaticationData data = await AuthenticationApiRH.Login(loginRequest);
                if(data?.Token != null)
                {
                    status = ResponseStatus.Success;
                    //authenticate here
                    _authService.AuthenticateCustomer(data, HttpContext);
                   
                }
                return Json(status);

            }
            catch (Exception ex)
            {
                return Json(status);
            }

           
        }
    }
}
