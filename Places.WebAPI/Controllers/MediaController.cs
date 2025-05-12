using Microsoft.AspNetCore.Mvc;
using Places.BLL.DTO;
using Places.BLL.Interfaces;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Http;

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

    public IActionResult Index()
    {
        var mediaList = _mediaService.GetAllMedia();
        return View(mediaList);
    }

    public IActionResult Details(int id)
    {
        var media = _mediaService.GetMediaById(id);
        if (media == null)
        {
            return NotFound();
        }
        return View(media);
    }

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

            return RedirectToAction("Details", "Places", new { id = mediaDTO.PlaceId });
        }

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

    [HttpPost]
    public IActionResult Edit(MediaDTO mediaDto)
    {
        if (ModelState.IsValid)
        {
            _mediaService.UpdateMedia(mediaDto);
            return RedirectToAction("Index");
        }
        ViewBag.Places = _placeService.GetAllPlaces();
        return View(mediaDto);
    }

    public IActionResult Delete(int id)
    {
        var media = _mediaService.GetMediaById(id);
        if (media == null)
        {
            return NotFound();
        }
        return View(media);
    }

    [HttpPost, ActionName("Delete")]
    public IActionResult DeleteConfirmed(int id)
    {
        _mediaService.DeleteMedia(id);
        return RedirectToAction("Index");
    }
}