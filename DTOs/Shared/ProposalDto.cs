using System;
using System.ComponentModel.DataAnnotations;

namespace artifi.Api.DTOs.Shared
{
    public class ProposalDto
    {
        [Required]
        public Guid JobPostId { get; set; }

        [Required]
        public string CoverLetter { get; set; }

        [Required]
        public decimal BidAmount { get; set; }
    }
}