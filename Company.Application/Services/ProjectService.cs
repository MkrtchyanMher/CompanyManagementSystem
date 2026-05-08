using Company.Application.Common;
using Company.Application.DTO.Project;
using Company.Application.Interfaces;
using Company.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Company.Application.Services
{
    public class ProjectService : BaseService<Project, ProjectDto, CreateProject, UpdateProjectDto>, IProjectService
    {
        public ProjectService(IApplicationDbContext context)
            : base(context, context.Projects)
        {
        }

        protected override ProjectDto MapToDto(Project entity) => new ProjectDto
        {
            Id = entity.Id,
            CompanyId = entity.CompanyId,
            Name = entity.Name,
            StartDate = entity.StartDate,
            EndDate = entity.EndDate
        };

        protected override Project MapToEntity(CreateProject dto) => new Project
        {
            CompanyId = dto.CompanyId,
            Name = dto.Name,
            StartDate = dto.StartDate,
            EndDate = dto.EndDate
        };

        protected override void UpdateEntity(UpdateProjectDto dto, Project entity)
        {
            entity.Name = dto.Name;
            entity.StartDate = dto.StartDate;
            entity.EndDate = dto.EndDate;
        }

        public override async Task<PagedResponse<ProjectDto>> GetAllPagedAsync(PagedRequest request)
        {
            var query = _dbSet.AsNoTracking();

            if (!string.IsNullOrWhiteSpace(request.FilterBy) && !string.IsNullOrWhiteSpace(request.FilterValue))
            {
                query = request.FilterBy.ToLower() switch
                {
                    "name" => query.Where(e => e.Name.Contains(request.FilterValue)),
                    _ => query
                };
            }

            if (!string.IsNullOrWhiteSpace(request.SortBy))
            {
                bool isDescending = request.SortOrder?.ToLower() == "desc";

                query = request.SortBy.ToLower() switch
                {
                    "name" => isDescending ? query.OrderByDescending(e => e.Name) : query.OrderBy(e => e.Name),
                    "startdate" => isDescending ? query.OrderByDescending(e => e.StartDate) : query.OrderBy(e => e.StartDate),
                    "enddate" => isDescending ? query.OrderByDescending(e => e.EndDate) : query.OrderBy(e => e.EndDate),
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

            return new PagedResponse<ProjectDto>
            {
                Data = data,
                Page = request.Page,
                PageSize = request.PageSize,
                TotalCount = totalCount
            };
        }
    }
}