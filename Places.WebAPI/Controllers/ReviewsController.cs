using Microsoft.AspNetCore.Mvc;
using Places.BLL.DTO;
using Places.BLL.Interfaces;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace Places.WebAPI.Controllers
{
    public class ReviewsController : Controller
    {
        private readonly IReviewService _reviewService;

        public ReviewsController(IReviewService reviewService)
        {
            _reviewService = reviewService;
        }

        public IActionResult Index()
        {
            var reviews = _reviewService.GetAllReviews();
            return View(reviews);
        }

        public IActionResult Details(int id)
        {
            var review = _reviewService.GetReviewById(id);
            if (review == null)
                return NotFound();
            return View(review);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Authorize]
        public IActionResult Create(ReviewDTO review)
        {
            if (ModelState.IsValid)
            {
                review.UserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
                _reviewService.AddReview(review);
                return RedirectToAction(nameof(Index));
            }
            return View(review);
        }

        public IActionResult Edit(int id)
        {
            var review = _reviewService.GetReviewById(id);
            if (review == null)
                return NotFound();
            return View(review);
        }

        [HttpPost]
        public IActionResult Edit(int id, ReviewDTO review)
        {
            if (id != review.Id)
                return BadRequest();
            if (ModelState.IsValid)
            {
                _reviewService.UpdateReview(review);
                return RedirectToAction(nameof(Index));
            }
            return View(review);
        }

        public IActionResult Delete(int id)
        {
            var review = _reviewService.GetReviewById(id);
            if (review == null)
                return NotFound();
            return View(review);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            _reviewService.DeleteReview(id);
            return RedirectToAction(nameof(Index));
        }
    }
}