using Places.BLL.DTO;
using Places.Models;

namespace Places.BLL.Mappers
{
    public class ReviewMapper
    {
        public ReviewDTO ToDto(Review review)
        {
            if (review == null) return null;
            return new ReviewDTO
            {
                Id = review.Id,
                Content = review.Content,
                PlaceId = review.PlaceId,
                UserId = review.UserId
            };
        }

        public Review ToEntity(ReviewDTO reviewDto)
        {
            if (reviewDto == null) return null;
            return new Review
            {
                Id = reviewDto.Id,
                Content = reviewDto.Content,
                PlaceId = reviewDto.PlaceId,
                UserId = reviewDto.UserId
            };
        }
    }
}