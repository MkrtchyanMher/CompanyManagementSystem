using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Company.Domain.Entities
{
    public class Company : BaseEntity
    {
        [Required]
        [MaxLength(100)]
        [RegularExpression(@"^\S+.*\S+$|^\S+$", ErrorMessage = "Name cannot be empty or whitespace")]
        public required string Name { get; set; }

        [Url(ErrorMessage = "Website url must be valid")]
        public string? Website { get; set; }
        public ICollection<Employee> Employees { get; set; } = new List<Employee>();
        public ICollection<Project> Projects { get; set; } = new List<Project>();
    }
}
