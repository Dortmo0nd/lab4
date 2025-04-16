using System.Collections.Generic;
using Places.BLL.DTO;

namespace Places.BLL.Interfaces
{
    public interface IReviewService
    {
        ReviewDTO GetReviewById(int id);
        IEnumerable<ReviewDTO> GetAllReviews();
        void AddReview(ReviewDTO review);
        void UpdateReview(ReviewDTO review);
        void DeleteReview(int id);
    }
}