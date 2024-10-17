using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using Data.Models;
using Data.Dtos;
using Newtonsoft.Json.Linq;
using System.IdentityModel.Tokens.Jwt;

namespace BookingSystem.Services
{
    public class AuthService : IAuthService
    {


        //[HttpPost]
        //public async void Login(LoginRequest user)
        //{
        //    var token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9lbWFpbGFkZHJlc3MiOiJrYXlAZXhhbXBsZS5jb20iLCJleHAiOjE3Mjg5Nzc5NTEsImlzcyI6Imh0dHBzOi8vc2FuZHJpbm8tZGV2LmF1dGgwLmNvbS8iLCJhdWQiOiJodHRwczovL2xvY2FsaG9zdDo3MjQ1LyJ9.IApQ0kVQGQ06RrGC3Jmr5tia9BQxHnoHNrABKroYvYc";
        //    var handler = new JwtSecurityTokenHandler();
        //    var jsonToken = handler.ReadToken(token) as JwtSecurityToken;

        //    var claims = jsonToken.Claims;
        //    var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        //    var principal = new ClaimsPrincipal(identity);
        //    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

        //}


        //public void AuthenticateCustomer(tbCustomer customer, HttpContext context)
        //{
        //    if (customer != null)
        //    {
        //        var identity = new[] {
        //        new Claim(ClaimTypes.NameIdentifier, customer.Id.ToString()),
        //        new Claim(ClaimTypes.Name,customer.Name),
        //        new Claim(ClaimTypes.Email,customer.Email),
        //        new Claim(ClaimTypes.MobilePhone,customer.Phone),
        //        };

        //        var claimsIdentity = new ClaimsIdentity(identity, CookieAuthenticationDefaults.AuthenticationScheme);
        //        var principle = new ClaimsPrincipal();
        //        principle.AddIdentity(claimsIdentity);

        //        context.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principle);

        //    }

        //}



        public void AuthenticateCustomer(AuthenticaticationData data, HttpContext context)
        {
            if (data.Token != null)
            {
                var handler = new JwtSecurityTokenHandler();
                var jsonToken = handler.ReadToken(data.Token) as JwtSecurityToken;

                var claims = jsonToken.Claims;
                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var principal = new ClaimsPrincipal(identity);
                context.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
            }

        }

        public void Logout(HttpContext context)
        {
            context.Session.Clear();
            context.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        }

        //public struct AuthDataIndex
        //{
        //    public const int Id = 0;
        //    public const int Name = 1;
        //    public const int Email = 2;
        //    public const int Phone = 3;
        //}
    }
}
