using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Places.BLL.DTO;
using Places.BLL.Interfaces;
using System.Security.Claims;
using System.Linq;

public class QuestionsController : Controller
{
    private readonly IQuestionService _questionService;
    private readonly IPlaceService _placeService;
    private readonly IUserService _userService;

    public QuestionsController(IQuestionService questionService, IPlaceService placeService, IUserService userService)
    {
        _questionService = questionService;
        _placeService = placeService;
        _userService = userService;
    }

    // WEB ACTIONS

    [HttpGet]
    public IActionResult Index()
    {
        var questions = _questionService.GetAllQuestions();
        ViewBag.Places = _placeService.GetAllPlaces().ToDictionary(p => p.Id, p => p.Name);
        return View(questions);
    }

    [HttpGet]
    [Authorize(Roles = "Admin,Manager")]
    public IActionResult Create()
    {
        ViewBag.Places = _placeService.GetAllPlaces().ToList();
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Admin,Manager")]
    public IActionResult Create(QuestionDTO questionDto)
    {
        if (ModelState.IsValid)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            _questionService.AddQuestion(questionDto, userId);
            return RedirectToAction("Index");
        }
        ViewBag.Places = _placeService.GetAllPlaces().ToList();
        return View(questionDto);
    }

    [HttpGet]
    [Authorize(Roles = "Admin,Manager")]
    public IActionResult Edit(int id)
    {
        var question = _questionService.GetQuestionById(id);
        if (question == null)
        {
            return NotFound();
        }
        ViewBag.Places = _placeService.GetAllPlaces().ToList();
        return View(question);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Admin,Manager")]
    public IActionResult Edit(QuestionDTO questionDto)
    {
        if (ModelState.IsValid)
        {
            try
            {
                _questionService.UpdateQuestion(questionDto);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, contact your system administrator.");
            }
        }
        ViewBag.Places = _placeService.GetAllPlaces().ToList();
        return View(questionDto);
    }

    [HttpPost]
    [Authorize(Roles = "Admin,Manager")]
    public IActionResult Delete(int id)
    {
        var question = _questionService.GetQuestionById(id);
        if (question == null)
        {
            return NotFound();
        }
        _questionService.DeleteQuestion(id);
        return RedirectToAction("Index");
    }

    [HttpGet]
    public IActionResult Details(int id)
    {
        var question = _questionService.GetQuestionById(id);
        if (question == null)
        {
            return NotFound();
        }
        ViewBag.PlaceName = _placeService.GetPlaceById(question.PlaceId)?.Name;
        return View(question);
    }

    // API ACTIONS

    [HttpGet("api/questions")]
    public IActionResult GetAll()
    {
        var questions = _questionService.GetAllQuestions();
        return Ok(questions);
    }

    [HttpGet("api/questions/{id}")]
    public IActionResult GetById(int id)
    {
        var question = _questionService.GetQuestionById(id);
        if (question == null)
        {
            return NotFound();
        }
        return Ok(question);
    }

    [HttpPost("api/questions")]
    [Authorize(Roles = "Admin,Manager")]
    public IActionResult CreateApi([FromBody] QuestionDTO questionDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
        _questionService.AddQuestion(questionDto, userId);
        return CreatedAtAction(nameof(GetById), new { id = questionDto.Id }, questionDto);
    }

    [HttpPut("api/questions/{id}")]
    [Authorize(Roles = "Admin,Manager")]
    public IActionResult UpdateApi(int id, [FromBody] QuestionDTO questionDto)
    {
        if (id != questionDto.Id)
        {
            return BadRequest("ID mismatch");
        }
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        var existingQuestion = _questionService.GetQuestionById(id);
        if (existingQuestion == null)
        {
            return NotFound();
        }
        try
        {
            _questionService.UpdateQuestion(questionDto);
            return NoContent();
        }
        catch (Exception)
        {
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpDelete("api/questions/{id}")]
    [Authorize(Roles = "Admin,Manager")]
    public IActionResult DeleteApi(int id)
    {
        var question = _questionService.GetQuestionById(id);
        if (question == null)
        {
            return NotFound();
        }
        _questionService.DeleteQuestion(id);
        return NoContent();
    }
}