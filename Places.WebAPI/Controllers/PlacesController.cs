using Microsoft.AspNetCore.Mvc;
using Places.BLL.DTO;
using Places.BLL.Interfaces;
using Microsoft.AspNetCore.Authorization;
using System.Linq;

namespace Places.WebAPI.Controllers
{
    public class PlacesController : Controller
    {
        private readonly IPlaceService _placeService;
        private readonly IUserService _userService;

        public PlacesController(IPlaceService placeService, IUserService userService)
        {
            _placeService = placeService;
            _userService = userService;
        }

        // GET: Places
        [HttpGet]
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

        [Authorize(Roles = "Admin")]
        public IActionResult Edit(int id)
        {
            var place = _placeService.GetPlaceById(id);
            if (place == null)
                return NotFound();
            return View(place);
        }

        [Authorize(Roles = "Admin")]
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

        [Authorize(Roles = "Admin")]
        public IActionResult Delete(int id)
        {
            var place = _placeService.GetPlaceById(id);
            if (place == null)
                return NotFound();
            return View(place);
        }
        
        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            _placeService.DeletePlace(id);
            return RedirectToAction(nameof(Index));
        }
        
        // api action
        
        [HttpGet("api/places")]
        public IActionResult GetAll()
        {
            var places = _placeService.GetAllPlaces();
            return Ok(places);
        }

        [HttpGet("api/places/{id}")]
        public IActionResult GetById(int id)
        {
            var place = _placeService.GetPlaceById(id);
            if (place == null)
            {
                return NotFound();
            }
            return Ok(place);
        }

        [HttpPost("api/places")]
        public IActionResult CreateApi([FromBody] PlaceDTO placeDto)
        {
            _placeService.AddPlace(placeDto);
            return CreatedAtAction(nameof(GetById), new { id = placeDto.Id }, placeDto);
        }

        [HttpPut("api/places/{id}")]
        public IActionResult UpdateApi(int id, [FromBody] PlaceDTO placeDto)
        {
            var existingPlace = _placeService.GetPlaceById(id);
            if (existingPlace == null)
            {
                return NotFound();
            }
            try
            {
                _placeService.UpdatePlace(placeDto);
                return NoContent();
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete("api/places/{id}")]
        public IActionResult DeleteApi(int id)
        {
            _placeService.DeletePlace(id);
            return NoContent();
        }
    }
}