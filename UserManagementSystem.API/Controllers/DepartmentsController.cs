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
        if(!ModelState.IsValid)
            return BadRequest(ModelState);
        
        try
        {
            var result = await _departmentService.AddAsync(departmentDto);

            if (result)
            {
                _logger.LogInformation("Department created successfully: DepartmentName={DepartmentName}", departmentDto.DepartmentName);
                return Created();
            }
            
            _logger.LogWarning("Department creation failed: DepartmentName={DepartmentName}", departmentDto.DepartmentName);
            return BadRequest("Department Not Created");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "AddDepartment failed: DepartmentName={DepartmentName}", departmentDto.DepartmentName);
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteDepartmentAsync(int departmentId)
    {
        
        if(departmentId <= 0)
            return BadRequest("Invalid Department Id");
        
        try
        {
            var result = await _departmentService.RemoveAsync(departmentId);
            
            if (result)
            {
                _logger.LogInformation("Department deleted successfully: DepartmentId={DepartmentId}", departmentId);
                return Ok();
            }
            
            _logger.LogWarning("Department deletion failed: DepartmentId={DepartmentId}", departmentId);
            return BadRequest("Department Not Deleted");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "DeleteDepartment failed: DepartmentId={DepartmentId}", departmentId);
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateDepartmentAsync(int id, DepartmentUpdateDto departmentDto)
    {
        if(!ModelState.IsValid)
            return BadRequest(ModelState);
        
        if (id <= 0)
            return BadRequest("Invalid Department Id");
        
        try
        {
            bool result = await _departmentService.UpdateAsync(id, departmentDto);
            
            if (result)
            {
                _logger.LogInformation("Department updated successfully: DepartmentId={DepartmentId}, NewDepartmentName={NewDepartmentName}", id, departmentDto.DepartmentName);
                return NoContent();
            }

            _logger.LogWarning("Department update failed - Department not found: DepartmentId={DepartmentId}", id);
            return NotFound("Department Not Found");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "UpdateDepartment failed: DepartmentId={DepartmentId}", id);
            return StatusCode(500, "Internal server error");
        }
    }
}