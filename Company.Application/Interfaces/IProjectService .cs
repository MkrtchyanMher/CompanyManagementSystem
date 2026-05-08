using Company.Application.Common;
using Company.Application.DTO.Project;

namespace Company.Application.Interfaces
{
    public interface IProjectService : IBaseService<ProjectDto, CreateProject, UpdateProjectDto>
    {
    }
}