using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Places.BLL.DTO;
using Places.BLL.Interfaces;
using System.Linq;
using System.Security.Claims;

namespace Places.WebAPI.Controllers
{
    public class ReviewsController : Controller
    {
        private readonly IReviewService _reviewService;
        private readonly IPlaceService _placeService;
        private readonly IUserService _userService;

        public ReviewsController(IReviewService reviewService, IPlaceService placeService, IUserService userService)
        {
            _reviewService = reviewService;
            _placeService = placeService;
            _userService = userService;
        }

        // GET: Reviews/Index
        public IActionResult Index()
        {
            var reviews = _reviewService.GetAllReviews();
            return View(reviews);
        }

        // GET: Reviews/Details/5
        public IActionResult Details(int id)
        {
            var review = _reviewService.GetReviewById(id);
            if (review == null) return NotFound();
            return View(review);
        }

        // GET: Reviews/Create
        [Authorize]
        public IActionResult Create(int? placeId)
        {
            ViewBag.Places = _placeService.GetAllPlaces().ToList();
            if (placeId.HasValue)
            {
                var review = new ReviewDTO { PlaceId = placeId.Value };
                return View(review);
            }
            return View(new ReviewDTO());
        }

        // POST: Reviews/Create
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public IActionResult Create(ReviewDTO review)
        {
            if (ModelState.IsValid)
            {
                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
                review.UserId = userId;
                _reviewService.AddReview(review);
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Places = _placeService.GetAllPlaces().ToList();
            return View(review);
        }

        // GET: Reviews/Edit/5
        [Authorize]
        public IActionResult Edit(int id)
        {
            var review = _reviewService.GetReviewById(id);
            if (review == null) return NotFound();
            if (review.UserId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value) && !User.IsInRole("Admin"))
                return Forbid();
            ViewBag.Places = _placeService.GetAllPlaces().ToList();
            return View(review);
        }

        // POST: Reviews/Edit
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(ReviewDTO review)
        {
            if (ModelState.IsValid)
            {
                _reviewService.UpdateReview(review);
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Places = _placeService.GetAllPlaces().ToList();
            return View(review);
        }

        // GET: Reviews/Delete/5
        [Authorize]
        public IActionResult Delete(int id)
        {
            var review = _reviewService.GetReviewById(id);
            if (review == null) return NotFound();
            if (review.UserId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value) && !User.IsInRole("Admin"))
                return Forbid();
            return View(review);
        }

        // POST: Reviews/Delete
        [HttpPost, ActionName("Delete")]
        [Authorize]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            _reviewService.DeleteReview(id);
            return RedirectToAction(nameof(Index));
        }
    }
}