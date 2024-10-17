using Data.Dtos;
using Data.Models;

namespace BookingSystem.Services
{
    public interface IAuthService
    {
        void AuthenticateCustomer(AuthenticaticationData data, HttpContext context);
       // void AuthenticateRestaurant(tbRestaurant restaurant, HttpContext context);
        void Logout(HttpContext context);
    }
}
