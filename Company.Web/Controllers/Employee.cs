using Company.Application.Common;
using Company.Application.DTO.Employee;
using Company.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Company.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class Employee : ControllerBase
    {
        private readonly IEmployeeService _service;

        public Employee(IEmployeeService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _service.GetAllAsync();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _service.GetByAsync(id);
            return Ok(result);
        }

        [HttpGet("paged")]
        public async Task<IActionResult> GetAllPaged(
            [FromQuery][Range(1, int.MaxValue)] int page = 1,
            [FromQuery][Range(1, 100)] int pageSize = 10,
            [FromQuery] string? sortBy = null,
            [FromQuery] string? sortOrder = "asc",
            [FromQuery] string? filterBy = null,
            [FromQuery] string? filterValue = null)
        {
            var request = new PagedRequest
            {
                Page = page,
                PageSize = pageSize,
                SortBy = sortBy,
                SortOrder = sortOrder ?? "asc",
                FilterBy = filterBy,
                FilterValue = filterValue
            };

            var result = await _service.GetAllPagedAsync(request);
            return Ok(result);
        }

        [HttpPut("bulk")]
        public async Task<IActionResult> BulkUpdate([FromBody] IEnumerable<BulkUpdateEmployee> dtos) 
        {
            await _service.BulkUpdateAsync(dtos);
            return NoContent();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateEmployee dto)
        {
            var result = await _service.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, UpdateEmployee dto)
        {
            var result = await _service.UpdateAsync(id, dto);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _service.DeleteByAsync(id);
            return NoContent();
        }
    }
}
