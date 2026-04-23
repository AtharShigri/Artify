using System;
using System.ComponentModel.DataAnnotations;

namespace Artify.Api.DTOs.Shared
{
    public class AgencyDto
    {
        [Required]
        [StringLength(100, ErrorMessage = "Agency name is too long.")]
        public string Name { get; set; }

        [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters.")]
        public string? Description { get; set; }

        public string? Website { get; set; }
    }
}