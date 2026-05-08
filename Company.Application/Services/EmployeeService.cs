using Company.Application.Common;
using Company.Application.DTO.Employee;
using Company.Application.Interfaces;
using Company.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Company.Application.Services
{
    public class EmployeeService : BaseService<Employee, EmployeeDto, CreateEmployee, UpdateEmployee>, IEmployeeService
    {
        public EmployeeService(IApplicationDbContext context)
            : base(context, context.Employees)
        {
        }

        protected override EmployeeDto MapToDto(Employee entity) => new EmployeeDto
        {
            Id = entity.Id,
            CompanyId = entity.CompanyId,
            FirstName = entity.FirstName,
            LastName = entity.LastName,
            Email = entity.Email,
            HireDate = entity.HireDate
        };

        protected override Employee MapToEntity(CreateEmployee dto) => new Employee
        {
            CompanyId = dto.CompanyId,
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            Email = dto.Email,
            HireDate = dto.HireDate
        };

        protected override void UpdateEntity(UpdateEmployee dto, Employee entity)
        {
            entity.FirstName = dto.FirstName;
            entity.LastName = dto.LastName;
            entity.Email = dto.Email;
            entity.HireDate = dto.HireDate;
        }

        public override async Task<PagedResponse<EmployeeDto>> GetAllPagedAsync(PagedRequest request)
        {
            var query = _dbSet.AsNoTracking();

            if (!string.IsNullOrWhiteSpace(request.FilterBy) && !string.IsNullOrWhiteSpace(request.FilterValue))
            {
                query = request.FilterBy.ToLower() switch
                {
                    "firstname" => query.Where(e => e.FirstName.Contains(request.FilterValue)),
                    "lastname" => query.Where(e => e.LastName.Contains(request.FilterValue)),
                    "email" => query.Where(e => e.Email.Contains(request.FilterValue)),
                    _ => query
                };
            }

            if (!string.IsNullOrWhiteSpace(request.SortBy))
            {
                bool isDescending = request.SortOrder?.ToLower() == "desc";

                query = request.SortBy.ToLower() switch
                {
                    "firstname" => isDescending ? query.OrderByDescending(e => e.FirstName) : query.OrderBy(e => e.FirstName),
                    "lastname" => isDescending ? query.OrderByDescending(e => e.LastName) : query.OrderBy(e => e.LastName),
                    "email" => isDescending ? query.OrderByDescending(e => e.Email) : query.OrderBy(e => e.Email),
                    "hiredate" => isDescending ? query.OrderByDescending(e => e.HireDate) : query.OrderBy(e => e.HireDate),
                    _ => query.OrderBy(e => e.Id)
                };
            }
            else
            {
                query = query.OrderBy(e => e.Id);
            }

            var totalCount = await query.CountAsync();

            var entities = await query
                .Skip((request.Page - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToListAsync();

            var data = entities.Select(MapToDto).ToList();

            return new PagedResponse<EmployeeDto>
            {
                Data = data,
                Page = request.Page,
                PageSize = request.PageSize,
                TotalCount = totalCount
            };
        }

        public async Task BulkUpdateAsync(IEnumerable<BulkUpdateEmployee> dtos)
        {
            await using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var employeeIds = dtos.Select(d => d.Id).ToList();
                var employees = await _dbSet
                    .Where(e => employeeIds.Contains(e.Id))
                    .ToListAsync();

                if (employees.Count != dtos.Count())
                    throw new KeyNotFoundException("Some employees were not found");

                foreach (var dto in dtos)
                {
                    var emp = employees.First(e => e.Id == dto.Id);
                    emp.FirstName = dto.FirstName;
                    emp.LastName = dto.LastName;
                    emp.Email = dto.Email;
                    emp.HireDate = dto.HireDate;
                }

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
    }
}