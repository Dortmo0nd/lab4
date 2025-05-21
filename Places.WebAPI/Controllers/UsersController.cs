using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Places.BLL.DTO;
using Places.BLL.Interfaces;

//[Authorize]
public class UsersController : Controller
{
    private readonly IUserService _userService;

    public UsersController(IUserService userService)
    {
        _userService = userService;
    }

    // WEB ACTIONS

    [HttpGet]
    public IActionResult Index()
    {
        var users = _userService.GetAllUsers();
        return View(users);
    }

    [HttpGet]
    [Authorize(Roles = "Admin")]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Admin")]
    public IActionResult Create(UserDTO userDto)
    {
        if (ModelState.IsValid)
        {
            _userService.AddUser(userDto);
            return RedirectToAction("Index");
        }
        return View(userDto);
    }

    [HttpGet]
    [Authorize(Roles = "Admin")]
    public IActionResult Edit(int id)
    {
        var user = _userService.GetUserById(id);
        if (user == null)
        {
            return NotFound();
        }
        return View(user);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Admin")]
    public IActionResult Edit(UserDTO userDto)
    {
        if (ModelState.IsValid)
        {
            try
            {
                _userService.UpdateUser(userDto);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, contact your system administrator.");
            }
        }
        return View(userDto);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public IActionResult Delete(int id)
    {
        var user = _userService.GetUserById(id);
        if (user == null)
        {
            return NotFound();
        }
        _userService.DeleteUser(id);
        return RedirectToAction("Index");
    }

    [HttpGet]
    public IActionResult Details(int id)
    {
        var user = _userService.GetUserById(id);
        if (user == null)
        {
            return NotFound();
        }
        return View(user);
    }

    // API ACTIONS

    [HttpGet("api/users")]
    public IActionResult GetAll()
    {
        var users = _userService.GetAllUsers();
        return Ok(users);
    }

    [HttpGet("api/users/{id}")]
    public IActionResult GetById(int id)
    {
        var user = _userService.GetUserById(id);
        if (user == null)
        {
            return NotFound();
        }
        return Ok(user);
    }

    [HttpPost("api/users")]
    public IActionResult CreateApi([FromBody] UserDTO userDto)
    {
        _userService.AddUser(userDto);
        return CreatedAtAction(nameof(GetById), new { id = userDto.Id }, userDto);
    }

    [HttpPut("api/users/{id}")]
    public IActionResult UpdateApi(int id, [FromBody] UserDTO userDto)
    {

        var existingUser = _userService.GetUserById(id);
        try
        {
            _userService.UpdateUser(userDto);
            return NoContent();
        }
        catch (Exception)
        {
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpDelete("api/users/{id}")]
    public IActionResult DeleteApi(int id)
    {
        var user = _userService.GetUserById(id);
        _userService.DeleteUser(id);
        return NoContent();
    }
}