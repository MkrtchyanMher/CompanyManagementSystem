using System.ComponentModel.DataAnnotations;

namespace Company.Application.DTO.Employee
{
    public record CreateEmployee
    {
        [Required]
        public Guid CompanyId { get; set; }

        [Required]
        [MaxLength(50)]
        public required string FirstName { get; set; }

        [Required]
        [MaxLength(50)]
        public required string LastName { get; set; }

        [Required]
        [EmailAddress]
        public required string Email { get; set; }

        public DateTime HireDate { get; set; }
    }
}
