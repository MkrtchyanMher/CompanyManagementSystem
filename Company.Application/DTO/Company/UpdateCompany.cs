using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Company.Application.DTO.Company
{
    public record UpdateCompany
    {
        [Required]
        [MaxLength(100)]
        public required string Name { get; set; }

        [Url(ErrorMessage = "Website url must be valid")]
        public string? Website { get; set; }
    }
}
