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
        private readonly IPlaceService _placeService; // Додаємо це

        public QuestionsController(IQuestionService questionService, IPlaceService placeService)
        {
            _questionService = questionService;
            _placeService = placeService; // Ініціалізуємо
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
            ViewBag.Places = _placeService.GetAllPlaces();
            return View();
        }

        [HttpPost]
        public IActionResult Create(QuestionDTO question, string PlaceName)
        {
            var place = _placeService.GetPlaceByName(PlaceName);
            if (place != null && ModelState.IsValid)
            {
                question.PlaceId = place.Id;
                _questionService.AddQuestion(question, 1); // Припускаємо userId = 1
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Places = _placeService.GetAllPlaces();
            ModelState.AddModelError("", "Місце не знайдене.");
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