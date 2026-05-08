using Company.Application.Common;
using Company.Application.DTO.Company;
using Company.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Company.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class Company : ControllerBase
    {
        private readonly ICompanyService _service;

        public Company(ICompanyService service)
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
        public async Task<ActionResult> GetAllPaged(
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

        [HttpPost]
        public async Task<IActionResult> Create(CreateCompany dto)
        {
            var result = await _service.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = result.Id  }, result);

        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, UpdateCompany dto)
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
