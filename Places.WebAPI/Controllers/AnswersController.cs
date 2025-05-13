using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Places.BLL.DTO;
using Places.BLL.Interfaces;
using System.Security.Claims;
using System.Linq;

namespace Places.WebAPI.Controllers
{
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

        // GET: Answers
        public IActionResult Index()
        {
            var answers = _answerService.GetAllAnswers();
            var questions = _questionService.GetAllQuestions().ToDictionary(q => q.Id, q => q.Content);
            var users = _userService.GetAllUsers().ToDictionary(u => u.Id, u => u.Full_name);
            ViewBag.Questions = questions;
            ViewBag.Users = users;
            return View(answers);
        }

        // GET: Answers/Details/5
        public IActionResult Details(int id)
        {
            var answer = _answerService.GetAnswerById(id);
            if (answer == null)
                return NotFound();
            var question = _questionService.GetQuestionById(answer.QuestionId);
            var user = _userService.GetUserById(answer.UserId);
            ViewBag.QuestionContent = question?.Content ?? "Невідоме питання";
            ViewBag.UserName = user?.Full_name ?? "Невідомий користувач";
            return View(answer);
        }

        // GET: Answers/Create
        public IActionResult Create(int? questionId)
        {
            ViewBag.Questions = _questionService.GetAllQuestions();
            return View(new AnswerDTO { QuestionId = questionId ?? 0 });
        }

        // POST: Answers/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(AnswerDTO answer)
        {
            if (ModelState.IsValid)
            {
                answer.UserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
                _answerService.AddAnswer(answer);
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Questions = _questionService.GetAllQuestions();
            return View(answer);
        }

        // GET: Answers/Edit/5
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var answer = _answerService.GetAnswerById(id);
            if (answer == null)
                return NotFound();
            // Перевірка прав доступу
            if (!IsOwnerOrAdmin(answer.UserId))
                return Forbid();
            ViewBag.Questions = _questionService.GetAllQuestions();
            return View(answer);
        }

        // POST: Answers/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(AnswerDTO answer)
        {
            // Перевірка прав доступу
            if (!IsOwnerOrAdmin(answer.UserId))
                return Forbid();
            if (ModelState.IsValid)
            {
                _answerService.UpdateAnswer(answer);
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Questions = _questionService.GetAllQuestions();
            return View(answer);
        }

        // GET: Answers/Delete/5
        [HttpGet]
        public IActionResult Delete(int id)
        {
            var answer = _answerService.GetAnswerById(id);
            if (answer == null)
                return NotFound();
            // Перевірка прав доступу
            if (!IsOwnerOrAdmin(answer.UserId))
                return Forbid();
            return View(answer);
        }

        // POST: Answers/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var answer = _answerService.GetAnswerById(id);
            // Перевірка прав доступу
            if (!IsOwnerOrAdmin(answer.UserId))
                return Forbid();
            _answerService.DeleteAnswer(id);
            return RedirectToAction(nameof(Index));
        }

        private bool IsOwnerOrAdmin(int userId)
        {
            var currentUserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var isAdmin = User.IsInRole("Admin");
            return isAdmin || currentUserId == userId;
        }
    }
}