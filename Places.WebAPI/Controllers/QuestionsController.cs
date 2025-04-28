using Microsoft.AspNetCore.Mvc;
using Places.BLL.DTO;
using Places.BLL.Interfaces;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace Places.WebAPI.Controllers
{
    public class QuestionsController : Controller
    {
        private readonly IQuestionService _questionService;

        public QuestionsController(IQuestionService questionService)
        {
            _questionService = questionService;
        }

        public IActionResult Index()
        {
            var questions = _questionService.GetAllQuestions();
            return View(questions);
        }

        public IActionResult Details(int id)
        {
            var question = _questionService.GetQuestionById(id);
            if (question == null)
                return NotFound();
            return View(question);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Authorize]
        public IActionResult Create(QuestionDTO question)
        {
            if (ModelState.IsValid)
            {
                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
                _questionService.AddQuestion(question, userId);
                return RedirectToAction(nameof(Index));
            }
            return View(question);
        }

        public IActionResult Edit(int id)
        {
            var question = _questionService.GetQuestionById(id);
            if (question == null)
                return NotFound();
            return View(question);
        }

        [HttpPost]
        public IActionResult Edit(int id, QuestionDTO question)
        {
            if (id != question.Id)
                return BadRequest();
            if (ModelState.IsValid)
            {
                _questionService.UpdateQuestion(question);
                return RedirectToAction(nameof(Index));
            }
            return View(question);
        }

        public IActionResult Delete(int id)
        {
            var question = _questionService.GetQuestionById(id);
            if (question == null)
                return NotFound();
            return View(question);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            _questionService.DeleteQuestion(id);
            return RedirectToAction(nameof(Index));
        }
    }
}