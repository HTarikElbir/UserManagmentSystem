using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UserManagementSystem.Business.Dtos.Department;
using UserManagementSystem.Business.Interfaces;

namespace UserManagementSystem.API.Controllers;

[Authorize(Roles = "Admin")]
[Route("api/departments")] 
[ApiController] 
public class DepartmentsController : ControllerBase
{
    private readonly IDepartmentService _departmentService;
    private readonly ILogger<DepartmentsController> _logger;

    public DepartmentsController(IDepartmentService departmentService, ILogger<DepartmentsController> logger)
    {
        _departmentService = departmentService;
        _logger = logger;
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
        {
            _logger.LogWarning("Invalid department ID: {DepartmentId}", id);
            return BadRequest("Invalid Department Id");
        }

        var result = await _departmentService.GetByIdAsync(id);
        
        return Ok(result);
    }

    [HttpGet("by-name/{name}")]
    public async Task<IActionResult> GetDepartmentByNameAsync(string name)
    {
        if(string.IsNullOrEmpty(name))
        {
            _logger.LogWarning("Invalid department name: {DepartmentName}", name);
            return BadRequest("Invalid Department Name");
        }
        
        var result = await _departmentService.GetByNameAsync(name);
        
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> AddDepartmentAsync(DepartmentAddDto departmentDto)
    {
        if(!ModelState.IsValid)
        {
            _logger.LogWarning("Invalid model state for add department: {DepartmentName}", departmentDto.DepartmentName);
            return BadRequest(ModelState);
        }
        
        var result = await _departmentService.AddAsync(departmentDto);

        if (result)
        {
            return Created();
        }
        
        return BadRequest("Department Not Created");
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteDepartmentAsync(int departmentId)
    {
        
        if(departmentId <= 0)
        {
            _logger.LogWarning("Invalid department ID for deletion: {DepartmentId}", departmentId);
            return BadRequest("Invalid Department Id");
        }
        
        var result = await _departmentService.RemoveAsync(departmentId);
        
        if (result)
        {
            return Ok();
        }
        
        return BadRequest("Department Not Deleted");
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateDepartmentAsync(int id, DepartmentUpdateDto departmentDto)
    {
        if(!ModelState.IsValid)
        {
            _logger.LogWarning("Invalid model state for update department: {DepartmentId}", id);
            return BadRequest(ModelState);
        }
        
        if (id <= 0)
        {
            _logger.LogWarning("Invalid department ID for update: {DepartmentId}", id);
            return BadRequest("Invalid Department Id");
        }
        
        var result = await _departmentService.UpdateAsync(id, departmentDto);
        
        if (result)
        {
            return NoContent();
        }

        return NotFound("Department Not Found");
    }
}