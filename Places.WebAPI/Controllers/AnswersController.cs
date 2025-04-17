using Microsoft.AspNetCore.Mvc;
using Places.BLL.DTO;
using Places.BLL.Interfaces;

namespace Places.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AnswersController : ControllerBase
    {
        private readonly IAnswerService _answerService;

        public AnswersController(IAnswerService answerService)
        {
            _answerService = answerService;
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var answer = _answerService.GetAnswerById(id);
            return answer != null ? Ok(answer) : NotFound();
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var answers = _answerService.GetAllAnswers();
            return Ok(answers);
        }

        [HttpPost]
        public IActionResult Post([FromBody] AnswerDTO answer)
        {
            _answerService.AddAnswer(answer);
            return CreatedAtAction(nameof(Get), new { id = answer.Id }, answer);
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] AnswerDTO answer)
        {
            if (id != answer.Id) return BadRequest();
            _answerService.UpdateAnswer(answer);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            _answerService.DeleteAnswer(id);
            return NoContent();
        }
    }
}