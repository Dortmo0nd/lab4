using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Places.BLL.DTO;
using Places.BLL.Interfaces;
using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Linq;

namespace Places.WebAPI.Controllers
{
    [Authorize]
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
            var mediaList = _mediaService.GetAllMedia();
            ViewBag.Places = _placeService.GetAllPlaces().ToDictionary(p => p.Id, p => p.Name);
            ViewBag.Users = _userService.GetAllUsers().ToDictionary(u => u.Id, u => u.Full_name);
            return View(mediaList);
        }

        // GET: Media/Details/5
        public IActionResult Details(int id)
        {
            var media = _mediaService.GetMediaById(id);
            if (media == null)
            {
                return NotFound();
            }
            ViewBag.PlaceName = _placeService.GetPlaceById(media.PlaceId ?? 0)?.Name ?? "Немає";
            ViewBag.UserName = _userService.GetUserById(media.UserId ?? 0)?.Full_name ?? "Немає";
            return View(media);
        }

        // GET: Media/Create
        public IActionResult Create()
        {
            ViewBag.Places = _placeService.GetAllPlaces();
            return View();
        }

        // POST: Media/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(MediaDTO mediaDto, IFormFile file)
        {
            if (ModelState.IsValid)
            {
                if (file != null && file.Length > 0)
                {
                    var filePath = Path.Combine("wwwroot/uploads", file.FileName);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }
                    mediaDto.FilePath = "/uploads/" + file.FileName;
                }
                else
                {
                    ModelState.AddModelError("", "Файл не завантажено.");
                    ViewBag.Places = _placeService.GetAllPlaces();
                    return View(mediaDto);
                }

                mediaDto.UserId = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier).Value);
                _mediaService.AddMedia(mediaDto);
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Places = _placeService.GetAllPlaces();
            return View(mediaDto);
        }

        // GET: Media/Edit/5
        public IActionResult Edit(int id)
        {
            var media = _mediaService.GetMediaById(id);
            if (media == null)
            {
                return NotFound();
            }
            ViewBag.Places = _placeService.GetAllPlaces();
            return View(media);
        }

        // POST: Media/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(MediaDTO mediaDto, IFormFile file)
        {
            if (mediaDto == null)
            {
                return BadRequest("Дані медіа-файлу відсутні.");
            }

            if (string.IsNullOrEmpty(mediaDto.Type) || mediaDto.PlaceId == null)
            {
                return BadRequest("Тип або місце медіа-файлу не вказано.");
            }

            if (file != null && file.Length > 0)
            {
                var filePath = Path.Combine("wwwroot/uploads", file.FileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }
                mediaDto.FilePath = "/uploads/" + file.FileName;
            }
            else if (string.IsNullOrEmpty(mediaDto.FilePath))
            {
                return BadRequest("Файл не завантажено, і шлях до файлу відсутній.");
            }

            try
            {
                _mediaService.UpdateMedia(mediaDto);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Помилка при оновленні медіа: " + ex.Message);
                ViewBag.Places = _placeService.GetAllPlaces();
                return View(mediaDto);
            }
        }

        // GET: Media/Delete/5
        public IActionResult Delete(int id)
        {
            var media = _mediaService.GetMediaById(id);
            if (media == null)
            {
                return NotFound();
            }
            return View(media);
        }

        // POST: Media/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            _mediaService.DeleteMedia(id);
            return RedirectToAction(nameof(Index));
        }
    }
}