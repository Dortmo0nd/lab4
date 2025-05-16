using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using NSubstitute;
using NUnit.Framework;
using AutoFixture;
using Ninject;
using Places.Abstract;
using Places.BLL.DTO;
using Places.BLL.Mappers;
using Places.BLL.Services;
using Places.Models;
using Assert = NUnit.Framework.Assert;

namespace Places.test_services
{
    [TestFixture]
    public class ReviewServiceTests
    {
        private ReviewService _reviewService;
        private IUnitOfWork _unitOfWork;
        private ReviewMapper _reviewMapper;
        private Fixture _fixture;
        protected IKernel Kernel { get; private set; }

        [SetUp]
        public void SetUp()
        {
            Kernel = new StandardKernel();
            _unitOfWork = Substitute.For<IUnitOfWork>();
            _reviewMapper = new ReviewMapper();
            _fixture = new Fixture();
            _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
                .ForEach(b => _fixture.Behaviors.Remove(b));
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());

            _unitOfWork.ReviewRepository.Returns(Substitute.For<IRepository<Review>>());

            Rebind<IUnitOfWork>(_unitOfWork);
            Rebind<ReviewMapper>(_reviewMapper);

            _reviewService = new ReviewService(_unitOfWork, _reviewMapper);
        }

        protected void Rebind<T>(T instance)
        {
            Kernel.Rebind<T>().ToConstant(instance);
        }

        [Test]
        public void GetReviewById_ExistingId_ReturnsReviewDto()
        {
            var review = _fixture.Build<Review>()
                .With(r => r.Id, 1)
                .With(r => r.Content, "Great place!")
                .With(r => r.PlaceId, 1)
                .With(r => r.UserId, 1)
                .Create();
            _unitOfWork.ReviewRepository.GetById(1).Returns(review);

            var result = _reviewService.GetReviewById(1);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Id, Is.EqualTo(1));
            Assert.That(result.Content, Is.EqualTo("Great place!"));
        }

        [Test]
        public void GetReviewById_NonExistingId_ReturnsNull()
        {
            _unitOfWork.ReviewRepository.GetById(999).Returns((Review)null);

            var result = _reviewService.GetReviewById(999);

            Assert.That(result, Is.Null);
        }

        [Test]
        public void GetAllReviews_ReturnsAllReviews()
        {
            var reviews = _fixture.CreateMany<Review>(3).ToList();
            _unitOfWork.ReviewRepository.GetAll().Returns(reviews);

            var result = _reviewService.GetAllReviews();

            Assert.That(result.Count(), Is.EqualTo(3));
        }

        [Test]
        public void AddReview_ValidReviewDto_AddsReview()
        {
            var reviewDto = _fixture.Build<ReviewDTO>()
                .With(r => r.Id, 0)
                .With(r => r.Content, "Nice place")
                .With(r => r.PlaceId, 1)
                .With(r => r.UserId, 1)
                .Create();

            _reviewService.AddReview(reviewDto);

            _unitOfWork.ReviewRepository.Received(1).Add(Arg.Any<Review>());
            _unitOfWork.Received(1).SaveChanges();
        }

        [Test]
        public void AddReview_InvalidReviewDto_ThrowsArgumentException()
        {
            var reviewDto = _fixture.Build<ReviewDTO>()
                .With(r => r.Content, "")
                .With(r => r.PlaceId, 0)
                .With(r => r.UserId, 0)
                .Create();

            Assert.Throws<ArgumentException>(() => _reviewService.AddReview(reviewDto));
        }

        [Test]
        public void UpdateReview_ValidReviewDto_UpdatesReview()
        {
            var review = _fixture.Build<Review>()
                .With(r => r.Id, 1)
                .Create();
            var reviewDto = _fixture.Build<ReviewDTO>()
                .With(r => r.Id, 1)
                .With(r => r.Content, "Updated review")
                .With(r => r.PlaceId, 1)
                .With(r => r.UserId, 1)
                .Create();
            _unitOfWork.ReviewRepository.GetById(1).Returns(review);

            _reviewService.UpdateReview(reviewDto);

            _unitOfWork.ReviewRepository.Received(1).Update(Arg.Any<Review>());
            _unitOfWork.Received(1).SaveChanges();
        }

        [Test]
        public void UpdateReview_InvalidReviewDto_ThrowsArgumentException()
        {
            var reviewDto = _fixture.Build<ReviewDTO>()
                .With(r => r.Content, "")
                .With(r => r.PlaceId, 0)
                .With(r => r.UserId, 0)
                .Create();

            Assert.Throws<ArgumentException>(() => _reviewService.UpdateReview(reviewDto));
        }

        [Test]
        public void DeleteReview_ExistingId_DeletesReview()
        {
            var review = _fixture.Build<Review>()
                .With(r => r.Id, 1)
                .Create();
            _unitOfWork.ReviewRepository.GetById(1).Returns(review);

            _reviewService.DeleteReview(1);

            _unitOfWork.ReviewRepository.Received(1).Delete(review);
            _unitOfWork.Received(1).SaveChanges();
        }
    }
}