using Data.Dtos;
using Data.Models;

namespace API.Services.Restaurant
{
    public interface IRestaurant
    {
        Task<ResponseData> UpSert(tbRestaurant restaurant);

        Task<tbRestaurant> GetById(int id);
    }
}
