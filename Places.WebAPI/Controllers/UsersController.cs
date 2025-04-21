using Microsoft.AspNetCore.Mvc;
using Places.BLL.DTO;
using Places.BLL.Interfaces;

namespace Places.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var user = _userService.GetUserById(id);
            return user != null ? Ok(user) : NotFound();
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var users = _userService.GetAllUsers();
            return Ok(users);
        }

        [HttpPost]
        public IActionResult Post([FromBody] UserDTO user)
        {
            _userService.AddUser(user);
            return CreatedAtAction(nameof(Get), new { id = user.Id }, user);
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] UserDTO user)
        {
            if (id != user.Id) return BadRequest();
            _userService.UpdateUser(user);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            _userService.DeleteUser(id);
            return NoContent();
        }
        
        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginDTO login)
        {
            if (_userService.VerifyPassword(login.UserId, login.Password))
                return Ok("Login successful");
            else
                return Unauthorized("Invalid credentials");
        }
    }
}