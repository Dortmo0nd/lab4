using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Places.BLL.DTO;
using Places.BLL.Interfaces;
using System.Security.Claims;

public class AnswersController : Controller
{
    private readonly IAnswerService _answerService;
    private readonly IQuestionService _questionService;
    private readonly IUserService _userService;

    public AnswersController(IAnswerService answerService, IQuestionService questionService, IUserService userService)
    {
        _answerService = answerService;
        _questionService = questionService;
        _userService = userService;
    }

    // WEB ACTIONS

    [HttpGet]
    public IActionResult Index()
    {
        var answers = _answerService.GetAllAnswers();
        ViewBag.Questions = _questionService.GetAllQuestions().ToDictionary(q => q.Id, q => q.Content);
        ViewBag.Users = _userService.GetAllUsers().ToDictionary(u => u.Id, u => u.Full_name);
        return View(answers);
    }

    [HttpGet]
    public IActionResult Create()
    {
        ViewBag.Questions = _questionService.GetAllQuestions().ToList();
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(AnswerDTO answerDto)
    {
        if (ModelState.IsValid)
        {
            answerDto.UserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            _answerService.AddAnswer(answerDto);
            return RedirectToAction("Index");
        }
        ViewBag.Questions = _questionService.GetAllQuestions().ToList();
        return View(answerDto);
    }

    [HttpGet]
    public IActionResult Edit(int id)
    {
        var answer = _answerService.GetAnswerById(id);
        if (answer == null)
        {
            return NotFound();
        }
        ViewBag.Questions = _questionService.GetAllQuestions().ToList();
        return View(answer);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(AnswerDTO answerDto)
    {
        if (ModelState.IsValid)
        {
            try
            {
                _answerService.UpdateAnswer(answerDto);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, contact your system administrator.");
            }
        }
        ViewBag.Questions = _questionService.GetAllQuestions().ToList();
        return View(answerDto);
    }

    [HttpPost]
    public IActionResult Delete(int id)
    {
        var answer = _answerService.GetAnswerById(id);
        if (answer == null)
        {
            return NotFound();
        }
        var currentUserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
        if (!User.IsInRole("Admin") && answer.UserId != currentUserId)
        {
            return Forbid();
        }
        _answerService.DeleteAnswer(id);
        return RedirectToAction("Index");
    }

    [HttpGet]
    public IActionResult Details(int id)
    {
        var answer = _answerService.GetAnswerById(id);
        if (answer == null)
        {
            return NotFound();
        }
        ViewBag.QuestionContent = _questionService.GetQuestionById(answer.QuestionId)?.Content;
        return View(answer);
    }

    // API ACTIONS

    [HttpGet("api/answers")]
    public IActionResult GetAll()
    {
        var answers = _answerService.GetAllAnswers();
        return Ok(answers);
    }

    [HttpGet("api/answers/{id}")]
    public IActionResult GetById(int id)
    {
        var answer = _answerService.GetAnswerById(id);
        if (answer == null)
        {
            return NotFound();
        }
        return Ok(answer);
    }

    [HttpPost("api/answers")]
    public IActionResult CreateApi([FromBody] AnswerDTO answerDto)
    {
        if (answerDto.UserId == 0)
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            answerDto.UserId = userIdString != null ? int.Parse(userIdString) : 1;
        }
        _answerService.AddAnswer(answerDto);
        return CreatedAtAction(nameof(GetById), new { id = answerDto.Id }, answerDto);
    }

    [HttpPut("api/answers/{id}")]
    public IActionResult UpdateApi(int id, [FromBody] AnswerDTO answerDto)
    {
        // Перевірка існування відповіді
        var existingAnswer = _answerService.GetAnswerById(id);
        if (existingAnswer == null)
        {
            return NotFound();
        }

        try
        {
            // Перевірка існування питання
            var question = _questionService.GetQuestionById(answerDto.QuestionId);
            if (question == null)
            {
                return BadRequest("Question with specified ID does not exist");
            }

            // Перевірка існування користувача
            var user = _userService.GetUserById(answerDto.UserId);
            if (user == null)
            {
                return BadRequest("User with specified ID does not exist");
            }

            // Оновлення відповіді
            _answerService.UpdateAnswer(answerDto);
            return NoContent();
        }
        catch (Exception ex)
        {
            // Логування помилки (можна замінити на ваш логер, наприклад, ILogger)
            Console.WriteLine($"Error updating answer: {ex.Message}\nStackTrace: {ex.StackTrace}");
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [HttpDelete("api/answers/{id}")]
    public IActionResult DeleteApi(int id)
    {
        var answer = _answerService.GetAnswerById(id);
        if (answer == null)
        {
            return NotFound();
        }
        _answerService.DeleteAnswer(id);
        return NoContent();
    }
}