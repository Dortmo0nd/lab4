using Microsoft.AspNetCore.Mvc;
using Places.BLL.DTO;
using Places.BLL.Interfaces;
using System.Linq;

namespace Places.WebAPI.Controllers
{
    public class PlacesController : Controller
    {
        private readonly IPlaceService _placeService;

        public PlacesController(IPlaceService placeService)
        {
            _placeService = placeService;
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
            if (place == null)
                return NotFound();
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
        public IActionResult Create(PlaceDTO place)
        {
            if (ModelState.IsValid)
            {
                _placeService.AddPlace(place);
                return RedirectToAction(nameof(Index));
            }
            return View(place);
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