using Microsoft.AspNetCore.Mvc;
using Places.BLL.DTO;
using Places.BLL.Interfaces;

namespace Places.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewsController : ControllerBase
    {
        private readonly IReviewService _reviewService;

        public ReviewsController(IReviewService reviewService)
        {
            _reviewService = reviewService;
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var review = _reviewService.GetReviewById(id);
            return review != null ? Ok(review) : NotFound();
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var reviews = _reviewService.GetAllReviews();
            return Ok(reviews);
        }

        [HttpPost]
        public IActionResult Post([FromBody] ReviewDTO review)
        {
            _reviewService.AddReview(review);
            return CreatedAtAction(nameof(Get), new { id = review.Id }, review);
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] ReviewDTO review)
        {
            if (id != review.Id) return BadRequest();
            _reviewService.UpdateReview(review);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            _reviewService.DeleteReview(id);
            return NoContent();
        }
    }
}