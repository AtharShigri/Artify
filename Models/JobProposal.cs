using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace artifi.Api.Models
{
public class JobProposal
{
    public Guid Id { get; set; }
    public Guid JobPostId { get; set; }
    public virtual JobPost JobPost { get; set; }

    public Guid ApplicantId { get; set; }
    public virtual ApplicationUser Applicant { get; set; }

    public string CoverLetter { get; set; }
    public decimal BidAmount { get; set; }
    public string Status { get; set; } // Pending, Accepted, Rejected
}
}