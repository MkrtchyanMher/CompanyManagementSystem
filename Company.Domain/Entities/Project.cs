using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Company.Domain.Entities
{
    public class Project : BaseEntity
    {
        public Guid CompanyId { get; set; }
        public Company? Company { get; set; }

        [Required]
        [MaxLength(100)]
        [RegularExpression(@"^\S+.*\S+$|^\S+$", ErrorMessage = "Name cannot be empty or whitespace")]
        public required string Name { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public ICollection<EmployeeProject> EmployeeProjects { get; set; } = new List<EmployeeProject>();
    }
}
