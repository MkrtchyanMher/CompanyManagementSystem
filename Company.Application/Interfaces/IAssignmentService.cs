using Company.Application.DTO.Employee;
using Company.Application.DTO.Project;

namespace Company.Application.Interfaces
{
    public interface IAssignmentService
    {
        Task AssignAsync(Guid employeeId, Guid projectId);
        Task UnassignAsync(Guid employeeId, Guid projectId);
        Task<IEnumerable<ProjectDto>> GetProjectsByEmployeeAsync(Guid employeeId);
        Task<IEnumerable<EmployeeDto>> GetEmployeesByProjectAsync(Guid projectId);
    }
}
