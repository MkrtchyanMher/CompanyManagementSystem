using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Company.Domain.Entities
{
    public class Employee : BaseEntity
    {
        public Guid CompanyId { get; set; }
        public Company? Company { get; set; }

        [Required]
        [MaxLength(50)]
        [RegularExpression(@"^\S+.*\S+$|^\S+$", ErrorMessage = "FirstName cannot be empty or whitespace")]
        public required string FirstName { get; set; }

        [Required]
        [MaxLength(50)]
        [RegularExpression(@"^\S+.*\S+$|^\S+$", ErrorMessage = "LastName cannot be empty or whitespace")]
        public required string LastName { get; set; }

        [Required]
        [EmailAddress]
        public required string Email { get; set; }

        public DateTime HireDate { get; set; }
        public ICollection<EmployeeProject> EmployeeProjects { get; set; } = new List<EmployeeProject>();
    }
}
