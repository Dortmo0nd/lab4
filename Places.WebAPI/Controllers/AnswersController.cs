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

        public AnswersController(IAnswerService answerService)
        {
            _answerService = answerService;
        }

        public IActionResult Index()
        {
            var answers = _answerService.GetAllAnswers();
            return View(answers);
        }

        public IActionResult Details(int id)
        {
            var answer = _answerService.GetAnswerById(id);
            if (answer == null)
                return NotFound();
            return View(answer);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Authorize]
        public IActionResult Create(AnswerDTO answer)
        {
            if (ModelState.IsValid)
            {
                answer.UserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
                _answerService.AddAnswer(answer);
                return RedirectToAction(nameof(Index));
            }
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