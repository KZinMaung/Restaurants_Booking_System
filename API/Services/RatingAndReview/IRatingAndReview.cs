using Data.Dtos;
using Data.Models;
using Data.ViewModels;
using Infra.Services;

namespace API.Services.RatingAndReview
{
    public interface IRatingAndReview
    {
        Task<Model<RatingAndReviewVM>> GetList(int resId, int page, int pageSize);

        Task<ResponseData> UpSert(tbRatingAndReview ratingNreview);
    }
}
