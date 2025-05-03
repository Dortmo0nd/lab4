using Microsoft.AspNetCore.Mvc;
using Places.BLL.DTO;
using Places.BLL.Interfaces;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace Places.WebAPI.Controllers
{
    [Authorize]
    public class QuestionsController : Controller
    {
        private readonly IQuestionService _questionService;
        private readonly IPlaceService _placeService;

        public QuestionsController(IQuestionService questionService, IPlaceService placeService)
        {
            _questionService = questionService;
            _placeService = placeService;
        }

        public IActionResult Index()
        {
            var questions = _questionService.GetAllQuestions();
            return View(questions);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            ViewBag.Places = _placeService.GetAllPlaces();
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult Create(QuestionDTO question, string PlaceName)
        {
            var place = _placeService.GetPlaceByName(PlaceName);
            if (place != null && ModelState.IsValid)
            {
                question.PlaceId = place.Id;
                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
                _questionService.AddQuestion(question, userId);
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Places = _placeService.GetAllPlaces();
            ModelState.AddModelError("", "Місце не знайдене.");
            return View(question);
        }
    }
}