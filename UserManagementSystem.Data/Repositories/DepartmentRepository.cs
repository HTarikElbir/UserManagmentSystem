using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using UserManagementSystem.Data.Contexts;
using UserManagementSystem.Data.Entities;
using UserManagementSystem.Data.Extensions;
using UserManagementSystem.Data.Interfaces;

namespace UserManagementSystem.Data.Repositories;

public class DepartmentRepository : IDepartmentRepository
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<DepartmentRepository> _logger;

    public DepartmentRepository(ApplicationDbContext context, ILogger<DepartmentRepository> logger)
    {
        _context = context;
        _logger = logger;
    }
    
    public async Task<List<Department>> GetAllAsync(int page = 1, int pageSize = 10) => await _context.Departments
        .AsNoTracking()
        .Paginate(page,pageSize)
        .ToListAsync();

    public async Task<Department?> GetByIdAsync(int id) => await _context.Departments.FirstOrDefaultAsync(d => d.DepartmentId == id);
   
    public async Task<Department?> GetByNameAsync(string name) => await _context.Departments.FirstOrDefaultAsync(d => d.DepartmentName.ToLower() == name.ToLower());
    

    public async Task AddAsync(Department department)
    {
        try
        {
            _logger.LogInformation("Adding new department: {DepartmentName}", department.DepartmentName);
            
            await _context.Departments.AddAsync(department);
            await _context.SaveChangesAsync();
            
            _logger.LogInformation("Department added successfully: {DepartmentName}, DepartmentId: {DepartmentId}", 
                department.DepartmentName, department.DepartmentId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to add department: {DepartmentName}", department.DepartmentName);
            throw;
        }
    }

    public async Task UpdateAsync(Department department)
    {
        try
        {
            _logger.LogInformation("Updating department: {DepartmentId}, DepartmentName: {DepartmentName}", 
                department.DepartmentId, department.DepartmentName);
            
            _context.Departments.Update(department);
            await _context.SaveChangesAsync();
            
            _logger.LogInformation("Department updated successfully: {DepartmentId}", department.DepartmentId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to update department: {DepartmentId}", department.DepartmentId);
            throw;
        }
    }

    public async Task RemoveAsync(int id)
    {
        try
        {
            _logger.LogInformation("Removing department: {DepartmentId}", id);
            
            var department = await _context.Departments.FindAsync(id);
            _context.Departments.Remove(department!);
            await _context.SaveChangesAsync();
            
            _logger.LogInformation("Department removed successfully: {DepartmentId}", id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to remove department: {DepartmentId}", id);
            throw;
        }
    }
}