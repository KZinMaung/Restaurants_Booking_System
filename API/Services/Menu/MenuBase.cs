using Azure;
using Core.Extension;
using Data.Constants;
using Data.Dtos;
using Data.Model;
using Data.Models;
using Infra.BlobStorage;
using Infra.Services;
using Infra.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace API.Services.Menu
{
    public class MenuBase : IMenu
    {
        private readonly BookingSystemDbCotnext _context;
        private readonly IConfiguration _configuration;
        
        UnitOfWork _uow;


        public MenuBase(BookingSystemDbCotnext context, IConfiguration configuration)
        {
            this._context = context;
            this._uow = new UnitOfWork(_context);
            this._configuration = configuration;
            
        }

        public async Task<ResponseData> UpSert(tbMenu menu)
        {
            tbMenu entity;
            ResponseData response = new ResponseData();
            //update
            if (menu.Id != 0)
            {

                menu.Accesstime = MyExtension.GetLocalTime();
                entity = await _uow.menuRepo.UpdateAsync(menu);
            }
            //insert
            else
            {
                
                menu.Accesstime = MyExtension.GetLocalTime();
                menu.CreatedAt = MyExtension.GetLocalTime();
                entity = await _uow.menuRepo.InsertReturnAsync(menu);
            }


            response.Status = entity != null ? ResponseStatus.Success : ResponseStatus.Fail;
            return response;

        }



        public async Task<Model<tbMenu>> GetList(int resId, int page, int pageSize, string? sortVal = "Id", string? sortDir = "desc",
                        string? q = "")
        {
            Expression<Func<tbMenu, bool>> basicFilter = null, restaurantFilter = null;
            IQueryable<tbMenu> query = _uow.menuRepo.GetAll()
                                            .Where(a => a.IsDeleted != true).AsQueryable();
            if (!string.IsNullOrEmpty(q))
            {
                basicFilter = n => n.Name.Contains(q);
                query = query.Where(basicFilter);
            }

            if (resId != 0 )
            {
                restaurantFilter = n => n.RestaurantId == resId;
                query = query.Where(restaurantFilter);
            }

            query = SORTLIT<tbMenu>.Sort(query, sortVal, sortDir);
            var result = await PagingService<tbMenu>.getPaging(page, pageSize, query);
            return result;
        }


        public async Task<tbMenu> GetById(int id)
        {
            return await _uow.menuRepo.GetAll()
                .FirstOrDefaultAsync(a => a.Id == id && a.IsDeleted != true) ?? new tbMenu();
        }

        public async Task<ResponseData> Delete(int id)
        {
            ResponseData response = new ResponseData();

            tbMenu entity = await GetById(id);
            entity.IsDeleted = true;
            entity = await _uow.menuRepo.UpdateAsync(entity);
            response.Status = entity != null ? ResponseStatus.Success : ResponseStatus.Fail;
            return response;

        }


    }
}
