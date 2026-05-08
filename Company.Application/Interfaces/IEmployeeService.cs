using Company.Application.Common;
using Company.Application.DTO.Employee;

namespace Company.Application.Interfaces
{
    public interface IEmployeeService : IBaseService<EmployeeDto, CreateEmployee, UpdateEmployee>
    {
        Task BulkUpdateAsync(IEnumerable<BulkUpdateEmployee> dtos);
    }
}
