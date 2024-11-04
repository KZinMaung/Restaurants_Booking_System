using Data.Model;
using Data.Models;
using Data.ViewModels;
using Infra.Services;
using Infra.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Linq.Expressions;

namespace API.Services.RatingAndReview
{
    public class RatingAndReviewBase : IRatingAndReview
    {
        private readonly BookingSystemDbCotnext _context;
        UnitOfWork _uow;


        public RatingAndReviewBase(BookingSystemDbCotnext context)
        {
            this._context = context;
            this._uow = new UnitOfWork(_context);

        }

        public async Task<Model<RatingAndReviewVM>> GetList(int resId, int page, int pageSize)
        {
           
            Expression<Func<RatingAndReviewVM, bool>>  restaurantFilter = null;
           
            var ratingNreviews = _uow.ratingNReviewRepo.GetAll().Where(r => r.IsDeleted != true).AsQueryable();
            var customers = _uow.customerRepo.GetAll().Where(c => c.IsDeleted != true).AsQueryable();

            var query = (from rt in ratingNreviews
                      join c in customers on rt.CustomerId equals c.Id
                      select new RatingAndReviewVM
                      {
                          RatingAndReviewId = rt.Id,
                          RatingAndReview = rt,
                          CustomerName = c.Name,
                          CustomerPhotoUrl = c.PhotoUrl
                      }).AsQueryable();


            if (resId != 0)
            {
                restaurantFilter = vm => vm.RatingAndReview.RestaurantId == resId;
                query = query.Where(restaurantFilter);
            }

            var sortVal = "RatingAndReviewId";
            var sortDir = "desc";
            query = SORTLIT<RatingAndReviewVM>.Sort(query, sortVal, sortDir);
            var result = await PagingService<RatingAndReviewVM>.getPaging(page, pageSize, query);
            return result;
        }
    }
}
