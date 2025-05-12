using Microsoft.AspNetCore.Mvc;
using Places.BLL.DTO;
using Places.BLL.Interfaces;
using System.Linq;

namespace Places.WebAPI.Controllers
{
    public class PlacesController : Controller
    {
        private readonly IPlaceService _placeService;
        private readonly IReviewService _reviewService;
        private readonly IQuestionService _questionService;
        private readonly IMediaService _mediaService;
        private readonly IUserService _userService;

        public PlacesController(IPlaceService placeService, IReviewService reviewService, 
            IQuestionService questionService, IMediaService mediaService, IUserService userService)
        {
            _placeService = placeService;
            _reviewService = reviewService;
            _questionService = questionService;
            _mediaService = mediaService;
            _userService = userService;
        }

        // GET: Places
        public IActionResult Index()
        {
            var places = _placeService.GetAllPlaces();
            return View(places);
        }

        // GET: Places/Details/5
        public IActionResult Details(int id)
        {
            var place = _placeService.GetPlaceById(id);
            if (place == null) return NotFound();

            var userIds = place.Reviews.Select(r => r.UserId).Distinct().ToList();
            var users = _userService.GetAllUsers()
                .Where(u => userIds.Contains(u.Id))
                .ToDictionary(u => u.Id, u => u.Full_name);

            ViewBag.Users = users;

            return View(place);
        }

        // GET: Places/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Places/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(PlaceDTO placeDto)
        {
            if (ModelState.IsValid)
            {
                _placeService.AddPlace(placeDto);
                return RedirectToAction("Index");
            }
            return View(placeDto);
        }

        // GET: Places/Edit/5
        public IActionResult Edit(int id)
        {
            var place = _placeService.GetPlaceById(id);
            if (place == null)
                return NotFound();
            return View(place);
        }

        // POST: Places/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, PlaceDTO place)
        {
            if (id != place.Id)
                return BadRequest();
            if (ModelState.IsValid)
            {
                _placeService.UpdatePlace(place);
                return RedirectToAction(nameof(Index));
            }
            return View(place);
        }

        // GET: Places/Delete/5
        public IActionResult Delete(int id)
        {
            var place = _placeService.GetPlaceById(id);
            if (place == null)
                return NotFound();
            return View(place);
        }

        // POST: Places/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            _placeService.DeletePlace(id);
            return RedirectToAction(nameof(Index));
        }
    }
}