using Data.Dtos;
using Data.Models;
using Infra.Services;

namespace API.Services.Restaurant
{
    public interface IRestaurant
    {
        Task<ResponseData> UpSert(tbRestaurant restaurant);

        Task<tbRestaurant> GetById(int id);

        Task<Model<RestaurantWithRating>> GetTopRatedRestaurants(int page, int pageSize, string? q = "");

        Task<Model<RestaurantWithRating>> GetList(int page, int pageSize, string? sortVal = "Id", string? sortDir = "desc",
                             string? q = "");

    }
}
