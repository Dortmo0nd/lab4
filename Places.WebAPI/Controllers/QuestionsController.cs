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

        public QuestionsController(IQuestionService questionService, IPlaceService placeService)
        {
            _questionService = questionService;
            _placeService = placeService;
        }

        // GET: Questions
        public IActionResult Index()
        {
            var questions = _questionService.GetAllQuestions();
            var places = _placeService.GetAllPlaces().ToDictionary(p => p.Id, p => p.Name);
            ViewBag.Places = places;
            return View(questions);
        }

        // GET: Questions/Create
        public IActionResult Create(int? placeId)
        {
            ViewBag.Places = _placeService.GetAllPlaces();
            var model = new QuestionDTO { PlaceId = placeId ?? 0 };
            return View(model);
        }

        // POST: Questions/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(QuestionDTO questionDto)
        {
            if (ModelState.IsValid)
            {
                var userId = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier).Value);
                _questionService.AddQuestion(questionDto, userId);
                return RedirectToAction("Index");
            }
            ViewBag.Places = _placeService.GetAllPlaces();
            return View(questionDto);
        }

        // GET: Questions/Edit/5
        public IActionResult Edit(int id)
        {
            var question = _questionService.GetQuestionById(id);
            if (question == null)
            {
                return NotFound();
            }
            ViewBag.Places = _placeService.GetAllPlaces();
            return View(question);
        }

        // POST: Questions/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(QuestionDTO questionDto)
        {
            if (ModelState.IsValid)
            {
                _questionService.UpdateQuestion(questionDto);
                return RedirectToAction("Index");
            }
            ViewBag.Places = _placeService.GetAllPlaces();
            return View(questionDto);
        }

        // GET: Questions/Details/5
        public IActionResult Details(int id)
        {
            var question = _questionService.GetQuestionById(id);
            if (question == null)
            {
                return NotFound();
            }
            var place = _placeService.GetPlaceById(question.PlaceId);
            ViewBag.PlaceName = place?.Name ?? "Невідоме місце";
            return View(question);
        }

        // GET: Questions/Delete/5
        public IActionResult Delete(int id)
        {
            var question = _questionService.GetQuestionById(id);
            if (question == null)
            {
                return NotFound();
            }
            return View(question);
        }

        // POST: Questions/DeleteConfirmed/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            _questionService.DeleteQuestion(id);
            return RedirectToAction("Index");
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
        return Ok(question);
    }

    [HttpPost("api/questions")]
    public IActionResult CreateApi([FromBody] QuestionDTO questionDto)
    {
        var userId = 1;
        _questionService.AddQuestion(questionDto, userId);
        return CreatedAtAction(nameof(GetById), new { id = questionDto.Id }, questionDto);
    }

    [HttpPut("api/questions/{id}")]
    public IActionResult UpdateApi(int id, [FromBody] QuestionDTO questionDto)
    {
        var existingQuestion = _questionService.GetQuestionById(id);
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
    public IActionResult DeleteApi(int id)
    {
        _questionService.DeleteQuestion(id);
        return NoContent();
    }
}