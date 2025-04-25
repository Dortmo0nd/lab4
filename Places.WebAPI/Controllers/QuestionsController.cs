using Microsoft.AspNetCore.Mvc;
using Places.BLL.DTO;
using Places.BLL.Interfaces;

namespace Places.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuestionsController : ControllerBase
    {
        private readonly IQuestionService _questionService;

        public QuestionsController(IQuestionService questionService)
        {
            _questionService = questionService;
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var question = _questionService.GetQuestionById(id);
            return question != null ? Ok(question) : NotFound();
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var questions = _questionService.GetAllQuestions();
            return Ok(questions);
        }

        [HttpPost]
        public IActionResult Post([FromBody] QuestionDTO question, [FromQuery] int userId)
        {
            _questionService.AddQuestion(question, userId);
            return CreatedAtAction(nameof(Get), new { id = question.Id }, question);
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] QuestionDTO question)
        {
            if (id != question.Id) return BadRequest();
            _questionService.UpdateQuestion(question);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            _questionService.DeleteQuestion(id);
            return NoContent();
        }
    }
}