
using API.Services.Customer;
using Core.Extension;
using Data.Constants;
using Data.Dtos;
using Data.Model;
using Data.Models;
using Infra.BlobStorage;
using Infra.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace API.Services.Auth
{
    public class AuthBase : IAuth
    {

        private readonly BookingSystemDbCotnext _context;
        private readonly IConfiguration _configuration;
        private IAzureBlobStorage _azureBlobStorage;
        UnitOfWork _uow;

        public AuthBase(BookingSystemDbCotnext context, IAzureBlobStorage azureBlobStorage, IConfiguration configuration)
        {
            this._context = context;
            this._uow = new UnitOfWork(_context);
            this._configuration = configuration;
            this._azureBlobStorage = azureBlobStorage;
        }

        private bool VerifyPassword(string inputPassword, string storedHash)
        {

            return BCrypt.Net.BCrypt.Verify(inputPassword, storedHash);
        }

        private string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }





        //login 
        public async Task<AuthenticaticationData> Login(LoginRequest loginRequest)
        {
            var customer = new tbCustomer();
            var restaurant = new tbRestaurant();
            bool isValid = false;
            AuthenticaticationData data = new AuthenticaticationData();

            if (loginRequest.UserType == UserType.Customer)
            {
                data = await CustomerLogin(loginRequest);
                return data;
            }
            else 
            {
                data = await RestaurantLogin(loginRequest);
                return data;
            }

        }

        public async Task<AuthenticaticationData> CustomerLogin(LoginRequest loginRequest)
        {
            var customer = new tbCustomer();
            bool isValid = false;
            AuthenticaticationData data = new AuthenticaticationData();
            Claims claims = new Claims();

            customer = await _uow.customerRepo.GetAll().Where(a => a.Email == loginRequest.Email && a.IsDeleted != true).FirstOrDefaultAsync() ?? new tbCustomer();
            isValid = customer.Id != 0 && VerifyPassword(loginRequest.Password, customer.Password);
            if (isValid)
            {
                claims.Id = customer.Id;
                claims.Name = customer.Name;
                claims.Email = customer.Email;
                claims.Phone = customer.Phone;
                var token = GenerateJwtToken(claims);

                data.Token = token;
                return data;
            }
            else
            {
                data.Token = null;
                return data;
            }
        }

        public async Task<AuthenticaticationData> RestaurantLogin(LoginRequest loginRequest)
        {
            var restaurant = new tbRestaurant();
            bool isValid = false;
            AuthenticaticationData data = new AuthenticaticationData();
            Claims claims = new Claims();

            restaurant = await _uow.restaurantRepo.GetAll().Where(a => a.Email == loginRequest.Email && a.IsDeleted != true).FirstOrDefaultAsync() ?? new tbRestaurant();
            isValid = restaurant.Id != 0 && VerifyPassword(loginRequest.Password, restaurant.Password);
            if (isValid)
            {
                claims.Id = restaurant.Id;
                claims.Name = restaurant.Name;
                claims.Email = restaurant.Email;
                claims.Phone = restaurant.Phone;
                claims.ProfilePhotoUrl = restaurant.ProfilePhotoUrl;
                var token = GenerateJwtToken(claims);
               

                data.Token = token;
                return data;
            }
            else
            {
                data.Token = null;
                return data;
            }
        }



        private string GenerateJwtToken(Claims claimData)
        {
            var jwtSettings = _configuration.GetSection("Jwt");
            var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtSettings["Secret"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);


            var claims = new[]
            {
            new Claim(ClaimTypes.NameIdentifier, claimData.Id.ToString()),
            new Claim(ClaimTypes.Name, claimData.Name),
            new Claim(ClaimTypes.Email, claimData.Email),
            new Claim(ClaimTypes.MobilePhone, claimData.Phone),
            new Claim(ClaimTypes.Uri, claimData.ProfilePhotoUrl ?? "")

            };

            var token = new JwtSecurityToken(
                issuer: jwtSettings["Issuer"],
                audience: jwtSettings["Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }


    }
}
