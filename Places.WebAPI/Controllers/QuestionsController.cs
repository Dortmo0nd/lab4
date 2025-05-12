using System;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Places.BLL.DTO;
using Places.BLL.Interfaces;
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
        public IActionResult Create(int? placeId)
        {
            ViewBag.Places = _placeService.GetAllPlaces();
            if (placeId.HasValue)
            {
                ViewBag.SelectedPlaceId = placeId.Value;
            }
            return View("Create");
        }

        // POST: Questions/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public IActionResult Create(QuestionDTO questionDto, string PlaceName)
        {
            var place = _placeService.GetPlaceByName(PlaceName);
            if (place != null)
            {
                questionDto.PlaceId = place.Id;
                var userId = _userService.GetUserByUsername(User.Identity.Name).Id;
                _questionService.AddQuestion(questionDto, userId);
                return RedirectToAction(nameof(Index));
            }
            ModelState.AddModelError("", "Place not found");
            ViewBag.Places = _placeService.GetAllPlaces();
            return View("Create", questionDto);
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
        [Authorize(Roles = "Admin")]
        public IActionResult Edit(int id, QuestionDTO questionDto, string PlaceName)
        {
            if (id != questionDto.Id)
            {
                return BadRequest();
            }
            var place = _placeService.GetPlaceByName(PlaceName);
            if (place != null)
            {
                questionDto.PlaceId = place.Id;
                _questionService.UpdateQuestion(questionDto);
                return RedirectToAction(nameof(Index));
            }
            ModelState.AddModelError("", "Place not found");
            ViewBag.Places = _placeService.GetAllPlaces();
            return View("Edit", questionDto);
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