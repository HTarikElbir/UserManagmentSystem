using Microsoft.AspNetCore.Mvc;
using UserManagementSystem.Business.Interfaces;

namespace UserManagementSystem.API.Controllers;

[Route("api/departments")] 
[ApiController] 
public class DepartmentsController : ControllerBase
{
    private IDepartmentService _departmentService;

    public DepartmentsController(IDepartmentService departmentService)
    {
        _departmentService = departmentService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllDepartments(int page = 1, int pageSize = 10)
    {
        return Ok(await _departmentService.GetAllAsync(page, pageSize));
    }
    
}