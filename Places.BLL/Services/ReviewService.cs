using System;
using System.Collections.Generic;
using System.Linq;
using Places.BLL.DTO;
using Places.BLL.Interfaces;
using Places.BLL.Mappers;
using Places.Abstract;
using Places.Models;

namespace Places.BLL.Services
{
    public class ReviewService : IReviewService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ReviewMapper _mapper;

        public ReviewService(IUnitOfWork unitOfWork, ReviewMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public ReviewDTO GetReviewById(int id)
        {
            var review = _unitOfWork.ReviewRepository.GetById(id);
            return _mapper.ToDto(review);
        }

        public IEnumerable<ReviewDTO> GetAllReviews()
        {
            var reviews = _unitOfWork.ReviewRepository.GetAll();
            return reviews.Select(r => _mapper.ToDto(r));
        }

        public void AddReview(ReviewDTO reviewDto)
        {
            if (reviewDto == null || string.IsNullOrEmpty(reviewDto.Content) || reviewDto.PlaceId <= 0 || reviewDto.UserId <= 0)
                throw new ArgumentException("Invalid review data");

            var review = _mapper.ToEntity(reviewDto);
            _unitOfWork.ReviewRepository.Add(review);
            _unitOfWork.SaveChanges();
        }

        public void UpdateReview(ReviewDTO reviewDto)
        {
            if (reviewDto == null || string.IsNullOrEmpty(reviewDto.Content) || reviewDto.PlaceId <= 0 || reviewDto.UserId <= 0)
                throw new ArgumentException("Invalid review data");

            var review = _mapper.ToEntity(reviewDto);
            _unitOfWork.ReviewRepository.Update(review);
            _unitOfWork.SaveChanges();
        }

        public void DeleteReview(int id)
        {
            var review = _unitOfWork.ReviewRepository.GetById(id);
            if (review != null)
            {
                _unitOfWork.ReviewRepository.Delete(review);
                _unitOfWork.SaveChanges();
            }
        }
    }
}
