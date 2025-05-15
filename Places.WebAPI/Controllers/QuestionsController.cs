using System;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Places.BLL.DTO;
using Places.BLL.Interfaces;
using System.Security.Claims;
using Places.BLL.Mappers;

namespace Places.WebAPI.Controllers
{
    [Authorize(Roles = "Admin")]
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

        // GET: Questions
        public IActionResult Index()
        {
            var questions = _questionService.GetAllQuestions();
            var places = _placeService.GetAllPlaces().ToDictionary(p => p.Id, p => p.Name);
            ViewBag.Places = places;
            return View(questions);
        }

        // GET: Questions/Details/5
        public IActionResult Details(int id)
        {
            var question = _questionService.GetQuestionWithAnswersById(id);
            if (question == null)
            {
                return NotFound();
            }
            return View(question);
        }

        // GET: Questions/Create
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.Places = _placeService.GetAllPlaces();
            return View();
        }

        // POST: Questions/Create
        [Authorize] // Доступ лише для авторизованих користувачів
        [HttpPost]
        public IActionResult Create(QuestionDTO question)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Places = _placeService.GetAllPlaces(); // Повернення списку місць для форми
                return View(question);
            }

            // Отримання ідентифікатора авторизованого користувача
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out var userId))
            {
                // Якщо claim відсутній або не є числом, перенаправляємо на сторінку помилки
                return RedirectToAction("Error", "Home");
            }

            // Передача question і userId у сервіс
            _questionService.AddQuestion(question, userId);

            return RedirectToAction("Index");
        }

        // GET: Questions/Edit/5
        [Authorize(Roles = "Admin")]
        public IActionResult Edit(int id)
        {
            var question = _questionService.GetQuestionById(id);
            if (question == null)
            {
                return NotFound();
            }
            ViewBag.Places = _placeService.GetAllPlaces();
            return View("Edit", question);
        }

        // POST: Questions/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(QuestionDTO questionDto)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _questionService.UpdateQuestion(questionDto);
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Помилка при оновленні питання: " + ex.Message);
                }
            }
            ViewBag.Places = _placeService.GetAllPlaces().ToList(); // Для випадаючого списку
            return View(questionDto);
        }

        // GET: Questions/Delete/5
        [Authorize(Roles = "Admin")]
        public IActionResult Delete(int id)
        {
            var question = _questionService.GetQuestionById(id);
            if (question == null)
            {
                return NotFound();
            }
            return View("Delete", question);
        }

        // POST: Questions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public IActionResult DeleteConfirmed(int id)
        {
            _questionService.DeleteQuestion(id);
            return RedirectToAction(nameof(Index));
        }
    }
}