using Data.Models;
using Infra.Services;

namespace API.Services.RestaurantSchedule
{
    public interface IRestaurantSchedule
    {

        Task<List<tbRestaurantSchedule>> GetList(int resId);

    }
}
