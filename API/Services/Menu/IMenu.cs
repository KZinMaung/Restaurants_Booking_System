using Data.Dtos;
using Data.Models;
using Infra.Services;

namespace API.Services.Menu
{
    public interface IMenu
    {
        Task<ResponseData> UpSert(tbMenu menu);

        Task<Model<tbMenu>> GetList(int resId, int page, int pageSize, string? sortVal = "ID", string? sortDir = "asc",
                                string? q = "");

        Task<tbMenu> GetById(int id);

        Task<ResponseData> Delete(int id);

    }
}
