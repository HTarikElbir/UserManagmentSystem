using Microsoft.AspNetCore.Mvc;
using UserManagementSystem.Business.Dtos.Department;
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
    public async Task<IActionResult> GetAllDepartmentsAsync(int page = 1, int pageSize = 10)
    {
        return Ok(await _departmentService.GetAllAsync(page, pageSize));
    }

    [HttpGet("by-id/{id}")]
    public async Task<IActionResult> GetDepartmentByIdAsync(int id)
    {
        if (id <= 0)
            return BadRequest("Invalid Department Id");

        var result = await _departmentService.GetByIdAsync(id);
        
        return Ok(result);
    }

    [HttpGet("by-name/{name}")]
    public async Task<IActionResult> GetDepartmentByNameAsync(string name)
    {
        if(string.IsNullOrEmpty(name))
            return BadRequest("Invalid Department Name");
        
        var result = await _departmentService.GetByNameAsync(name);
        
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> AddDepartmentAsync(DepartmentAddDto departmentDto)
    {
        var result = await _departmentService.AddAsync(departmentDto);

        if (result)
            return Created();
        
        return BadRequest("Department Not Created");
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteDepartmentAsync(int departmentId)
    {
        if(departmentId <= 0)
            return BadRequest("Invalid Department Id");
        
        var result = await _departmentService.RemoveAsync(departmentId);
        
        if (result)
            return Ok();
        
        return BadRequest("Department Not Deleted"); // Maybe i can use NotFound()
    }
}