using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Places.BLL.DTO;
using Places.BLL.Interfaces;
using System.Security.Claims;

namespace Places.WebAPI.Controllers
{
    [Authorize] // Вимагаємо авторизацію для всіх дій
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

        // GET: Reviews
        public IActionResult Index()
        {
            var reviews = _reviewService.GetAllReviews();
            var places = _placeService.GetAllPlaces().ToDictionary(p => p.Id, p => p.Name);
            var users = _userService.GetAllUsers().ToDictionary(u => u.Id, u => u.Full_name);
            ViewBag.Places = places;
            ViewBag.Users = users;
            return View(reviews);
        }

        // GET: Reviews/Details/5
        public IActionResult Details(int id)
        {
            var review = _reviewService.GetReviewById(id);
            if (review == null)
                return NotFound();
            var place = _placeService.GetPlaceById(review.PlaceId);
            var user = _userService.GetUserById(review.UserId);
            ViewBag.PlaceName = place?.Name ?? "Невідоме місце";
            ViewBag.UserName = user?.Full_name ?? "Невідомий користувач";
            return View(review);
        }

        // GET: Reviews/Edit/5
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var review = _reviewService.GetReviewById(id);
            if (review == null)
                return NotFound();
            // Перевірка прав доступу
            if (!IsOwnerOrAdmin(review.UserId))
                return Forbid();
            ViewBag.Places = _placeService.GetAllPlaces();
            return View(review);
        }

        // POST: Reviews/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(ReviewDTO review)
        {
            // Перевірка прав доступу
            if (!IsOwnerOrAdmin(review.UserId))
                return Forbid();
            if (ModelState.IsValid)
            {
                _reviewService.UpdateReview(review);
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Places = _placeService.GetAllPlaces();
            return View(review);
        }

        // GET: Reviews/Delete/5
        [HttpGet]
        public IActionResult Delete(int id)
        {
            var review = _reviewService.GetReviewById(id);
            if (review == null)
                return NotFound();
            // Перевірка прав доступу
            if (!IsOwnerOrAdmin(review.UserId))
                return Forbid();
            return View(review);
        }

        // POST: Reviews/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var review = _reviewService.GetReviewById(id);
            // Перевірка прав доступу
            if (!IsOwnerOrAdmin(review.UserId))
                return Forbid();
            _reviewService.DeleteReview(id);
            return RedirectToAction(nameof(Index));
        }

        // GET: Reviews/Create
        public IActionResult Create(int? placeId)
        {
            ViewBag.Places = _placeService.GetAllPlaces();
            return View(new ReviewDTO { PlaceId = placeId ?? 0 });
        }

        // POST: Reviews/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(ReviewDTO review)
        {
            if (ModelState.IsValid)
            {
                review.UserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)); // Прив'язка до поточного користувача
                _reviewService.AddReview(review);
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Places = _placeService.GetAllPlaces();
            return View(review);
        }

        // Допоміжний метод для перевірки прав
        private bool IsOwnerOrAdmin(int userId)
        {
            var currentUserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var isAdmin = User.IsInRole("Admin");
            return isAdmin || currentUserId == userId;
        }
    }
}