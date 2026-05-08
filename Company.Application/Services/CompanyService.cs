using Company.Application.Common;
using Company.Application.DTO.Company;
using Company.Application.DTO.Employee;
using Company.Application.Interfaces;
using Microsoft.EntityFrameworkCore;
using CompanyDto = Company.Application.DTO.Company.Company;
using CompanyEntity = Company.Domain.Entities.Company;

namespace Company.Application.Services
{
    public class CompanyService : BaseService<CompanyEntity, CompanyDto, CreateCompany, UpdateCompany>, ICompanyService
    {
        public CompanyService(IApplicationDbContext context)
            : base(context, context.Companies)
        {
        }

        protected override CompanyDto MapToDto(CompanyEntity entity) => new CompanyDto
        {
            Id = entity.Id,
            Name = entity.Name,
            Website = entity.Website,
            CreateDate = entity.CreatedTime
        };

        protected override CompanyEntity MapToEntity(CreateCompany dto) => new CompanyEntity
        {
            Id = Guid.NewGuid(),
            Name = dto.Name,
            Website = dto.Website,
            CreatedTime = DateTime.UtcNow
        };

        protected override void UpdateEntity(UpdateCompany dto, CompanyEntity entity)
        {
            entity.Name = dto.Name;
            entity.Website = dto.Website;
        }

        public override async Task<PagedResponse<CompanyDto>> GetAllPagedAsync(PagedRequest request)
        {
            var query = _dbSet.AsNoTracking();

            if (!string.IsNullOrWhiteSpace(request.FilterBy) && !string.IsNullOrWhiteSpace(request.FilterValue))
            {
                query = request.FilterBy.ToLower() switch
                {
                    "name" => query.Where(e => e.Name.Contains(request.FilterValue)),
                    "website" => query.Where(e => e.Website != null && e.Website.Contains(request.FilterValue)),
                    _ => query
                };
            }

            if (!string.IsNullOrWhiteSpace(request.SortBy))
            {
                bool isDescending = request.SortOrder?.ToLower() == "desc";

                query = request.SortBy.ToLower() switch
                {
                    "name" => isDescending ? query.OrderByDescending(e => e.Name) : query.OrderBy(e => e.Name),
                    "website" => isDescending ? query.OrderByDescending(e => e.Website) : query.OrderBy(e => e.Website),
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

            return new PagedResponse<CompanyDto>
            {
                Data = data,
                Page = request.Page,
                PageSize = request.PageSize,
                TotalCount = totalCount
            };
        }
    }
}