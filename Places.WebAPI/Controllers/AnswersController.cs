using Microsoft.AspNetCore.Mvc;
using Places.BLL.DTO;
using Places.BLL.Interfaces;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace Places.WebAPI.Controllers
{
    public class AnswersController : Controller
    {
        private readonly IAnswerService _answerService;
        private readonly IQuestionService _questionService; // Додаємо це
        private readonly IUserService _userService; // Додаємо це

        public AnswersController(IAnswerService answerService, IQuestionService questionService, IUserService userService)
        {
            _answerService = answerService;
            _questionService = questionService; // Ініціалізуємо
            _userService = userService; // Ініціалізуємо
        }

        public IActionResult Index()
        {
            var answers = _answerService.GetAllAnswers();
            var questions = _questionService.GetAllQuestions().ToDictionary(q => q.Id, q => q.Content);
            var users = _userService.GetAllUsers().ToDictionary(u => u.Id, u => u.Full_name);
            ViewBag.Questions = questions;
            ViewBag.Users = users;
            return View(answers);
        }

        public IActionResult Details(int id)
        {
            var answer = _answerService.GetAnswerById(id);
            if (answer == null)
                return NotFound();
            var questionContent = _questionService.GetQuestionById(answer.QuestionId)?.Content ?? "Немає";
            var userName = _userService.GetUserById(answer.UserId)?.Full_name ?? "Немає";
            ViewBag.QuestionContent = questionContent;
            ViewBag.UserName = userName;
            return View(answer);
        }

        public IActionResult Create()
        {
            ViewBag.Questions = _questionService.GetAllQuestions();
            ViewBag.Users = _userService.GetAllUsers();
            return View();
        }

        [HttpPost]
        public IActionResult Create(AnswerDTO answer, string QuestionContent, string UserName)
        {
            var question = _questionService.GetQuestionByContent(QuestionContent);
            var user = _userService.GetUserByUsername(UserName);
            if (question != null && user != null && ModelState.IsValid)
            {
                answer.QuestionId = question.Id;
                answer.UserId = user.Id;
                _answerService.AddAnswer(answer);
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Questions = _questionService.GetAllQuestions();
            ViewBag.Users = _userService.GetAllUsers();
            ModelState.AddModelError("", "Питання або користувач не знайдені.");
            return View(answer);
        }

        public IActionResult Edit(int id)
        {
            var answer = _answerService.GetAnswerById(id);
            if (answer == null)
                return NotFound();
            return View(answer);
        }

        [HttpPost]
        public IActionResult Edit(int id, AnswerDTO answer)
        {
            if (id != answer.Id)
                return BadRequest();
            if (ModelState.IsValid)
            {
                _answerService.UpdateAnswer(answer);
                return RedirectToAction(nameof(Index));
            }
            return View(answer);
        }

        public IActionResult Delete(int id)
        {
            var answer = _answerService.GetAnswerById(id);
            if (answer == null)
                return NotFound();
            return View(answer);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            _answerService.DeleteAnswer(id);
            return RedirectToAction(nameof(Index));
        }
    }
}