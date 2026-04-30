using System;
using System.ComponentModel.DataAnnotations;

namespace artifi.Api.DTOs.Shared
{
    public class JobPostDto
    {
        [Required]
        [StringLength(100)]
        public string Title { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        [Range(1, 1000000)]
        public decimal Budget { get; set; }

        // Optional: Add Category if you want to filter jobs by art type
        public string? Category { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}