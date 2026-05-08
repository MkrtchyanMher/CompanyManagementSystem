using Company.Application.Interfaces;
using Company.Domain.Entities;
using Company.Application.DTO.Employee;
using Company.Application.DTO.Project;
using Microsoft.EntityFrameworkCore;

namespace Company.Application.Services
{
    public class AssignmentService : IAssignmentService
    {
        private readonly IApplicationDbContext _context;

        public AssignmentService(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AssignAsync(Guid employeeId, Guid projectId)
        {
            await using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var employee = await _context.Employees.FindAsync(employeeId);
                var project = await _context.Projects.FindAsync(projectId);

                if (employee == null || project == null)
                    throw new KeyNotFoundException("Employee or Project not found");

                if (employee.CompanyId != project.CompanyId)
                    throw new InvalidOperationException("Company mismatch");

                if (!await _context.EmployeeProjects.AnyAsync(ep => ep.EmployeeId == employeeId && ep.ProjectId == projectId))
                {
                    await _context.EmployeeProjects.AddAsync(new EmployeeProject
                    {
                        EmployeeId = employeeId,
                        ProjectId = projectId,
                        AssignedDate = DateTime.UtcNow
                    });

                    await _context.SaveChangesAsync();
                }

                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task UnassignAsync(Guid employeeId, Guid projectId)
        {
            await using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var assignment = await _context.EmployeeProjects
                    .FirstOrDefaultAsync(ep => ep.EmployeeId == employeeId && ep.ProjectId == projectId)
                    ?? throw new KeyNotFoundException("Assignment not found");

                _context.EmployeeProjects.Remove(assignment);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task<IEnumerable<ProjectDto>> GetProjectsByEmployeeAsync(Guid employeeId)
        {
            return await _context.EmployeeProjects
                .Where(ep => ep.EmployeeId == employeeId)
                .Select(ep => new ProjectDto
                {
                    Id = ep.Project.Id,
                    CompanyId = ep.Project.CompanyId,
                    Name = ep.Project.Name,
                    StartDate = ep.Project.StartDate,
                    EndDate = ep.Project.EndDate
                })
                .ToListAsync();
        }

        public async Task<IEnumerable<EmployeeDto>> GetEmployeesByProjectAsync(Guid projectId)
        {
            return await _context.EmployeeProjects
                .Where(ep => ep.ProjectId == projectId)
                .Select(ep => new EmployeeDto
                {
                    Id = ep.Employee.Id,
                    CompanyId = ep.Employee.CompanyId,
                    FirstName = ep.Employee.FirstName,
                    LastName = ep.Employee.LastName,
                    Email = ep.Employee.Email,
                    HireDate = ep.Employee.HireDate
                })
                .ToListAsync();
        }
    }
}