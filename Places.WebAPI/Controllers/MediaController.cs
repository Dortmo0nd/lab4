using Microsoft.AspNetCore.Mvc;
using Places.BLL.DTO;
using Places.BLL.Interfaces;

namespace Places.WebAPI.Controllers
{
    public class MediaController : Controller
    {
        private readonly IMediaService _mediaService;
        private readonly IPlaceService _placeService;
        private readonly IUserService _userService;

        public MediaController(IMediaService mediaService, IPlaceService placeService, IUserService userService)
        {
            _mediaService = mediaService;
            _placeService = placeService;
            _userService = userService;
        }

        // GET: Media
        public IActionResult Index()
        {
            var mediaFiles = _mediaService.GetAllMedia();
            ViewBag.Places = _placeService.GetAllPlaces().ToDictionary(p => p.Id, p => p.Name);
            ViewBag.Users = _userService.GetAllUsers().ToDictionary(u => u.Id, u => u.Full_name);
            return View(mediaFiles);
        }

        public IActionResult Details(int id)
        {
            var media = _mediaService.GetMediaById(id);
            if (media == null)
                return NotFound();
            var placeName = media.PlaceId.HasValue ? _placeService.GetPlaceById(media.PlaceId.Value)?.Name ?? "Немає" : "Немає";
            var userName = media.UserId.HasValue ? _userService.GetUserById(media.UserId.Value)?.Full_name ?? "Немає" : "Немає";
            ViewBag.PlaceName = placeName;
            ViewBag.UserName = userName;
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