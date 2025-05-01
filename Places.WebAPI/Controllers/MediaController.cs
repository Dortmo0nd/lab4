using Microsoft.AspNetCore.Mvc;
using Places.BLL.DTO;
using Places.BLL.Interfaces;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace Places.WebAPI.Controllers
{
    public class MediaController : Controller
    {
        private readonly IMediaService _mediaService;
        private readonly IPlaceService _placeService; // Додаємо це
        private readonly IUserService _userService; // Додаємо це

        public MediaController(IMediaService mediaService, IPlaceService placeService, IUserService userService)
        {
            _mediaService = mediaService;
            _placeService = placeService; // Ініціалізуємо
            _userService = userService; // Ініціалізуємо
        }

        public IActionResult Index()
        {
            var media = _mediaService.GetAllMedia();
            return View(media);
        }

        public IActionResult Details(int id)
        {
            var media = _mediaService.GetMediaById(id);
            if (media == null)
                return NotFound();
            return View(media);
        }

        public IActionResult Create()
        {
            ViewBag.Places = _placeService.GetAllPlaces();
            ViewBag.Users = _userService.GetAllUsers();
            return View();
        }

        [HttpPost]
        public IActionResult Create(MediaDTO media, string PlaceName, string UserName)
        {
            var place = _placeService.GetPlaceByName(PlaceName);
            var user = _userService.GetUserByUsername(UserName);
            if (place != null && user != null && ModelState.IsValid)
            {
                media.PlaceId = place.Id;
                media.UserId = user.Id;
                _mediaService.AddMedia(media);
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Places = _placeService.GetAllPlaces();
            ViewBag.Users = _userService.GetAllUsers();
            ModelState.AddModelError("", "Місце або користувач не знайдені.");
            return View(media);
        }

        public IActionResult Edit(int id)
        {
            var media = _mediaService.GetMediaById(id);
            if (media == null)
                return NotFound();
            return View(media);
        }

        [HttpPost]
        public IActionResult Edit(int id, MediaDTO media)
        {
            if (id != media.Id)
                return BadRequest();
            if (ModelState.IsValid)
            {
                _mediaService.UpdateMedia(media);
                return RedirectToAction(nameof(Index));
            }
            return View(media);
        }

        public IActionResult Delete(int id)
        {
            var media = _mediaService.GetMediaById(id);
            if (media == null)
                return NotFound();
            return View(media);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            _mediaService.DeleteMedia(id);
            return RedirectToAction(nameof(Index));
        }
    }
}