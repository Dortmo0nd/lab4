using Microsoft.AspNetCore.Mvc;
using Places.BLL.DTO;
using Places.BLL.Interfaces;

namespace Places.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MediaController : ControllerBase
    {
        private readonly IMediaService _mediaService;

        public MediaController(IMediaService mediaService)
        {
            _mediaService = mediaService;
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var media = _mediaService.GetMediaById(id);
            return media != null ? Ok(media) : NotFound();
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var media = _mediaService.GetAllMedia();
            return Ok(media);
        }

        [HttpPost]
        public IActionResult Post([FromBody] MediaDTO media)
        {
            _mediaService.AddMedia(media);
            return CreatedAtAction(nameof(Get), new { id = media.Id }, media);
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] MediaDTO media)
        {
            if (id != media.Id) return BadRequest();
            _mediaService.UpdateMedia(media);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            _mediaService.DeleteMedia(id);
            return NoContent();
        }
    }
}