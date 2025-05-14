using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Places.BLL.DTO;
using Places.BLL.Interfaces;
using System.Security.Claims;

[Authorize]
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
        if (answer == null || (!User.IsInRole("Admin") && answer.UserId != int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier))))
        {
            return Forbid();
        }
        ViewBag.Questions = _questionService.GetAllQuestions().ToList();
        return View(answer);
    }

    [HttpPost]
    public IActionResult Edit(AnswerDTO answerDto)
    {
        if (ModelState.IsValid)
        {
            _answerService.UpdateAnswer(answerDto);
            return RedirectToAction("Index");
        }
        ViewBag.Questions = _questionService.GetAllQuestions().ToList();
        return View(answerDto);
    }

    [HttpPost]
    public IActionResult Delete(int id)
    {
        var answer = _answerService.GetAnswerById(id);
        if (answer == null || (!User.IsInRole("Admin") && answer.UserId != int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier))))
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
}