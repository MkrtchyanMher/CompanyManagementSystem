using Company.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Company.Web.Controllers
{
    [ApiController]
    [Route("api/employees")]
    [Authorize]
    public class Assignment : ControllerBase
    {
        private readonly IAssignmentService _service;

        public Assignment(IAssignmentService service)
        {
            _service = service;
        }

        [HttpPost("{employeeId}/projects/{projectId}")]
        public async Task<IActionResult> Assign(Guid employeeId, Guid projectId)
        {
            await _service.AssignAsync(employeeId, projectId);
            return Ok();
        }

        [HttpDelete("{employeeId}/projects/{projectId}")]
        public async Task<IActionResult> Unassign(Guid employeeId, Guid projectId)
        {
            await _service.UnassignAsync(employeeId,projectId);
            return NoContent();
        }

        [HttpGet("{employeeId}/projects")]
        public async Task<IActionResult> GetProjectsByEmployee(Guid employeeId)
        {
            var result = await _service.GetProjectsByEmployeeAsync(employeeId);
            return Ok(result);
        }

        [HttpGet("{projectId}/employees")]
        public async Task<IActionResult> GetEmployeesByProject(Guid projectId)
        {
            var result = await _service.GetEmployeesByProjectAsync(projectId);
            return Ok(result);
        }

    }
}
