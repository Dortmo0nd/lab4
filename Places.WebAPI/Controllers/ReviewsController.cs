using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Places.BLL.DTO;
using Places.BLL.Interfaces;
using System.Security.Claims;

public class ReviewsController : Controller
{
    private readonly IReviewService _reviewService;
    private readonly IPlaceService _placeService;
    private readonly IUserService _userService;

    public ReviewsController(IReviewService reviewService, IPlaceService placeService, IUserService userService)
    {
        _reviewService = reviewService;
        _placeService = placeService;
        _userService = userService;
    }

    // WEB ACTIONS

    [HttpGet]
    public IActionResult Index()
    {
        var reviews = _reviewService.GetAllReviews();
        ViewBag.Places = _placeService.GetAllPlaces().ToDictionary(p => p.Id, p => p.Name);
        ViewBag.Users = _userService.GetAllUsers().ToDictionary(u => u.Id, u => u.Full_name);
        return View(reviews);
    }

    [HttpGet]
    public IActionResult Create()
    {
        ViewBag.Places = _placeService.GetAllPlaces().ToList();
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(ReviewDTO reviewDto)
    {
        if (ModelState.IsValid)
        {
            reviewDto.UserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            _reviewService.AddReview(reviewDto);
            return RedirectToAction("Index");
        }
        ViewBag.Places = _placeService.GetAllPlaces().ToList();
        return View(reviewDto);
    }

    [HttpGet]
    public IActionResult Edit(int id)
    {
        var review = _reviewService.GetReviewById(id);
        if (review == null)
        {
            return NotFound();
        }
        ViewBag.Places = _placeService.GetAllPlaces().ToList();
        return View(review);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(ReviewDTO reviewDto)
    {
        if (ModelState.IsValid)
        {
            try
            {
                _reviewService.UpdateReview(reviewDto);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, contact your system administrator.");
            }
        }
        ViewBag.Places = _placeService.GetAllPlaces().ToList();
        return View(reviewDto);
    }

    [HttpPost]
    public IActionResult Delete(int id)
    {
        var review = _reviewService.GetReviewById(id);
        if (review == null)
        {
            return NotFound();
        }
        var currentUserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
        if (!User.IsInRole("Admin") && review.UserId != currentUserId)
        {
            return Forbid();
        }
        _reviewService.DeleteReview(id);
        return RedirectToAction("Index");
    }

    [HttpGet]
    public IActionResult Details(int id)
    {
        var review = _reviewService.GetReviewById(id);
        if (review == null)
        {
            return NotFound();
        }
        ViewBag.PlaceName = _placeService.GetPlaceById(review.PlaceId)?.Name;
        ViewBag.UserName = _userService.GetUserById(review.UserId)?.Full_name;
        return View(review);
    }

    // API ACTIONS

    [HttpGet("api/reviews")]
    public IActionResult GetAll()
    {
        var reviews = _reviewService.GetAllReviews();
        return Ok(reviews);
    }

    [HttpGet("api/reviews/{id}")]
    public IActionResult GetById(int id)
    {
        var review = _reviewService.GetReviewById(id);
        if (review == null)
        {
            return NotFound();
        }
        return Ok(review);
    }

    [HttpPost("api/reviews")]
    public IActionResult CreateApi([FromBody] ReviewDTO reviewDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        reviewDto.UserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
        _reviewService.AddReview(reviewDto);
        return CreatedAtAction(nameof(GetById), new { id = reviewDto.Id }, reviewDto);
    }

    [HttpPut("api/reviews/{id}")]
    public IActionResult UpdateApi(int id, [FromBody] ReviewDTO reviewDto)
    {
        if (id != reviewDto.Id)
        {
            return BadRequest("ID mismatch");
        }
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        var existingReview = _reviewService.GetReviewById(id);
        if (existingReview == null)
        {
            return NotFound();
        }
        try
        {
            _reviewService.UpdateReview(reviewDto);
            return NoContent();
        }
        catch (Exception)
        {
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpDelete("api/reviews/{id}")]
    public IActionResult DeleteApi(int id)
    {
        var review = _reviewService.GetReviewById(id);
        if (review == null)
        {
            return NotFound();
        }
        var currentUserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
        if (!User.IsInRole("Admin") && review.UserId != currentUserId)
        {
            return Forbid();
        }
        _reviewService.DeleteReview(id);
        return NoContent();
    }
}