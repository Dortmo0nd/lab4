using Microsoft.AspNetCore.Mvc;
using Places.BLL.DTO;
using Places.BLL.Interfaces;

namespace Places.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        private readonly IRoleService _roleService;

        public RolesController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var role = _roleService.GetRoleById(id);
            return role != null ? Ok(role) : NotFound();
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var roles = _roleService.GetAllRoles();
            return Ok(roles);
        }

        [HttpPost]
        public IActionResult Post([FromBody] RoleDTO role)
        {
            _roleService.AddRole(role);
            return CreatedAtAction(nameof(Get), new { id = role.Id }, role);
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] RoleDTO role)
        {
            if (id != role.Id) return BadRequest();
            _roleService.UpdateRole(role);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            _roleService.DeleteRole(id);
            return NoContent();
        }
    }
}