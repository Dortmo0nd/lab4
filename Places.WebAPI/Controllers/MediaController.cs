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

        public MediaController(IMediaService mediaService)
        {
            _mediaService = mediaService;
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
            return View();
        }

        [HttpPost]
        [Authorize]
        public IActionResult Create(MediaDTO media)
        {
            if (ModelState.IsValid)
            {
                media.UserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
                _mediaService.AddMedia(media);
                return RedirectToAction(nameof(Index));
            }
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