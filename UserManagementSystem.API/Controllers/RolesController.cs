using Microsoft.AspNetCore.Mvc;
using UserManagementSystem.Business.Interfaces;

namespace UserManagementSystem.API.Controllers
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
        
        [HttpGet]
        public async Task<IActionResult> GetAllRolesAsync()
        {
            // Return the list of roles with a 200-OK status
            return Ok(await _roleService.GetAllRolesAsync());
        }
    }
}
