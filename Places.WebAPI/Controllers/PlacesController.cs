using Microsoft.AspNetCore.Mvc;
using Places.BLL.DTO;
using Places.BLL.Interfaces;

namespace Places.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlacesController : ControllerBase
    {
        private readonly IPlaceService _placeService;

        public PlacesController(IPlaceService placeService)
        {
            _placeService = placeService;
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var place = _placeService.GetPlaceById(id);
            return place != null ? Ok(place) : NotFound();
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var places = _placeService.GetAllPlaces();
            return Ok(places);
        }

        [HttpPost]
        public IActionResult Post([FromBody] PlaceDTO place)
        {
            _placeService.AddPlace(place);
            return CreatedAtAction(nameof(Get), new { id = place.Id }, place);
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] PlaceDTO place)
        {
            if (id != place.Id) return BadRequest();
            _placeService.UpdatePlace(place);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            _placeService.DeletePlace(id);
            return NoContent();
        }
    }
}