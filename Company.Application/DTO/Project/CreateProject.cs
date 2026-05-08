using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Company.Application.DTO.Project
{
    public record CreateProject
    {
        [Required]
        public Guid CompanyId { get; set; }

        [Required]
        [MaxLength(100)]
        public required string Name { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}
