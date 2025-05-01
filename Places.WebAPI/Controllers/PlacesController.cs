using Microsoft.AspNetCore.Mvc;
using Places.BLL.DTO;
using Places.BLL.Interfaces;

namespace Places.WebAPI.Controllers
{
    public class PlacesController : Controller
    {
        private readonly IPlaceService _placeService;

        public PlacesController(IPlaceService placeService)
        {
            _placeService = placeService;
        }

        public IActionResult Index()
        {
            var places = _placeService.GetAllPlaces();
            return View(places);
        }

        public IActionResult Details(int id)
        {
            var place = _placeService.GetPlaceById(id);
            if (place == null)
                return NotFound();
            return View(place);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(PlaceDTO place)
        {
            if (ModelState.IsValid)
            {
                _placeService.AddPlace(place);
                TempData["SuccessMessage"] = "Place created successfully.";
                return RedirectToAction(nameof(Index));
            }
            return View(place);
        }

        public IActionResult Edit(int id)
        {
            var place = _placeService.GetPlaceById(id);
            if (place == null)
                return NotFound();
            return View(place);
        }

        [HttpPost]
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

        public IActionResult Delete(int id)
        {
            var place = _placeService.GetPlaceById(id);
            if (place == null)
                return NotFound();
            return View(place);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            _placeService.DeletePlace(id);
            return RedirectToAction(nameof(Index));
        }
    }
}