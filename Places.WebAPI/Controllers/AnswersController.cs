using Microsoft.AspNetCore.Mvc;
using Places.BLL.DTO;
using Places.BLL.Interfaces;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace Places.WebAPI.Controllers
{
    [Authorize] // Вимагає авторизації для всіх дій контролера
    public class AnswersController : Controller
    {
        private readonly IAnswerService _answerService;
        private readonly IQuestionService _questionService;

        public AnswersController(IAnswerService answerService, IQuestionService questionService)
        {
            _answerService = answerService;
            _questionService = questionService;
        }

        // GET: Answers
        public IActionResult Index()
        {
            var answers = _answerService.GetAllAnswers();
            return View(answers);
        }

        // GET: Answers/Details/5
        public IActionResult Details(int id)
        {
            var answer = _answerService.GetAnswerById(id);
            if (answer == null)
            {
                return NotFound();
            }
            return View(answer);
        }

        // GET: Answers/Create
        public IActionResult Create()
        {
            var questions = _questionService.GetAllQuestions();
            ViewBag.Questions = questions ?? new List<QuestionDTO>(); // Захист від null
            return View();
        }

        // POST: Answers/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(AnswerDTO answer)
        {
            if (!ModelState.IsValid)
            {
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    Console.WriteLine("Validation error: " + error.ErrorMessage);
                }
                var questions = _questionService.GetAllQuestions();
                ViewBag.Questions = questions ?? new List<QuestionDTO>();
                return View(answer);
            }
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            answer.UserId = userId;
            _answerService.AddAnswer(answer);
            return RedirectToAction(nameof(Index));
        }

        // GET: Answers/Edit/5
        public IActionResult Edit(int id)
        {
            var answer = _answerService.GetAnswerById(id);
            if (answer == null)
            {
                return NotFound();
            }
            var questions = _questionService.GetAllQuestions();
            ViewBag.Questions = questions ?? new List<QuestionDTO>();
            return View(answer);
        }

        // POST: Answers/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, AnswerDTO answer)
        {
            if (id != answer.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                // Оновлення відповіді, UserId залишається незмінним
                _answerService.UpdateAnswer(answer);
                return RedirectToAction(nameof(Index));
            }
            var questions = _questionService.GetAllQuestions();
            ViewBag.Questions = questions ?? new List<QuestionDTO>();
            return View(answer);
        }

        // GET: Answers/Delete/5
        public IActionResult Delete(int id)
        {
            var answer = _answerService.GetAnswerById(id);
            if (answer == null)
            {
                return NotFound();
            }
            return View(answer);
        }

        // POST: Answers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            _answerService.DeleteAnswer(id);
            return RedirectToAction(nameof(Index));
        }
    }
}