using Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.ViewModels
{
    public class RatingAndReviewVM
    {
        public int RatingAndReviewId { get; set; }
        public tbRatingAndReview RatingAndReview { get; set; }
        public string? CustomerName { get; set; }
        public string? CustomerPhotoUrl { get; set; }

    }
}
