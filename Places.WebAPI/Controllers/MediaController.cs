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
        public IActionResult Create(int? placeId)
        {
            ViewBag.Places = _placeService.GetAllPlaces();
            return View(new MediaDTO { PlaceId = placeId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(MediaDTO mediaDTO, IFormFile file)
        {
            // Перевірка наявності файлу
            if (file == null || file.Length == 0)
            {
                ModelState.AddModelError("file", "Будь ласка, виберіть файл.");
                ViewBag.Places = _placeService.GetAllPlaces();
                return View(mediaDTO);
            }

            // Перевірка типу файлу
            var allowedImageTypes = new[] { "image/jpeg", "image/png", "image/gif" };
            if (!allowedImageTypes.Contains(file.ContentType) && mediaDTO.Type == "Фото")

            {
                ModelState.AddModelError("file", "Дозволені лише файли у форматах JPEG, PNG або GIF для фото.");
                ViewBag.Places = _placeService.GetAllPlaces();
                return View(mediaDTO);

            }

            // Перевірка розміру файлу (максимум 5 МБ)
            const long maxFileSize = 5 * 1024 * 1024; // 5 MB
            if (file.Length > maxFileSize)
            {
                ModelState.AddModelError("file", "Розмір файлу перевищує 5 МБ.");
                ViewBag.Places = _placeService.GetAllPlaces();
                return View(mediaDTO);
            }
            // Перевірка типу медіа
            if (string.IsNullOrEmpty(mediaDTO.Type))
            {
                ModelState.AddModelError("Type", "Будь ласка, виберіть тип медіа.");
                ViewBag.Places = _placeService.GetAllPlaces();
                return View(mediaDTO);
            }

            // Визначаємо шлях для збереження файлу
            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads");
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            // Унікальне ім’я файлу
            var uniqueFileName = $"{Guid.NewGuid()}_{file.FileName}";
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);

            try
            {
                // Зберігаємо файл
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(fileStream);
                }

                // Заповнюємо DTO
                mediaDTO.FilePath = $"/uploads/{uniqueFileName}";
                mediaDTO.UserId = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value); // Отримуємо ID користувача

                // Додаємо медіа через сервіс
                _mediaService.AddMedia(mediaDTO);
            }
            catch (Exception ex)
            {
                // Логування помилки (можна використати ILogger)
                ModelState.AddModelError("", $"Помилка при збереженні файлу: {ex.Message}");
                ViewBag.Places = _placeService.GetAllPlaces();
                return View(mediaDTO);
            }

            return RedirectToAction("Index", "Media", new { id = mediaDTO.PlaceId });
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