using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Artify.Api.Data;
using Artify.Api.Models;
using Artify.Api.DTOs.Shared;
using System.Security.Claims;

namespace Artify.Api.Controllers.Shared
{
    [Route("api/projectboard")]
    [ApiController]
    [Authorize]
    public class ProjectBoardController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ProjectBoardController(ApplicationDbContext context)
        {
            _context = context;
        }

        // ---------------- BUYER/AGENCY SECTION ----------------

        [HttpPost("post-job")]
        [Authorize(Roles = "Buyer,Admin")]
        public async Task<IActionResult> PostJob([FromBody] JobPostDto dto)
        {
            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var job = new JobPost
            {
                Id = Guid.NewGuid(),
                Title = dto.Title,
                Description = dto.Description,
                Budget = dto.Budget,
                PosterId = userId,
                CreatedAt = DateTime.UtcNow
            };

            _context.JobPosts.Add(job);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Project posted successfully", jobId = job.Id });
        }

        [HttpGet("my-jobs")]
        public async Task<IActionResult> GetMyJobs()
        {
            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            
            var jobs = await _context.JobPosts
                .Where(j => j.PosterId == userId)
                .Select(j => new {
                    j.Id,
                    j.Title,
                    j.Description,
                    j.Budget,
                    j.PosterId,
                    PosterName = j.Poster.FullName,
                    Proposals = j.Proposals.Select(p => new {
                        p.Id,
                        p.JobPostId,
                        p.ApplicantId,
                        ApplicantName = p.Applicant.FullName,
                        p.CoverLetter,
                        p.BidAmount,
                        p.Status
                    })
                })
                .ToListAsync();

            return Ok(jobs);
        }

        // ---------------- ARTIST/STUDIO SECTION ----------------

        [HttpGet("browse")]
        [AllowAnonymous] // Anyone can see open projects
        public async Task<IActionResult> BrowseJobs()
        {
            var jobs = await _context.JobPosts
                .Select(j => new {
                    j.Id,
                    j.Title,
                    j.Description,
                    j.Budget,
                    PosterName = j.Poster.FullName
                })
                .ToListAsync();

            return Ok(jobs);
        }

        [HttpPost("submit-proposal")]
        [Authorize(Roles = "Artist")]
        public async Task<IActionResult> SubmitProposal([FromBody] ProposalDto dto)
        {
            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            // Check if artist already applied
            var existingProposal = await _context.JobProposals
                .FirstOrDefaultAsync(p => p.JobPostId == dto.JobPostId && p.ApplicantId == userId);
            
            if (existingProposal != null)
                return BadRequest(new { message = "You have already applied for this job." });

            // Check if user is an artist/agency member
            var proposal = new JobProposal
            {
                Id = Guid.NewGuid(),
                JobPostId = dto.JobPostId,
                ApplicantId = userId,
                CoverLetter = dto.CoverLetter,
                BidAmount = dto.BidAmount,
                Status = "Pending"
            };

            _context.JobProposals.Add(proposal);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Proposal submitted successfully" });
        }

        // ---------------- MANAGEMENT SECTION ----------------

        [HttpPatch("proposals/{id}/status")]
        [Authorize(Roles = "Buyer,Admin")]
        public async Task<IActionResult> UpdateProposalStatus(Guid id, [FromBody] string status)
        {
            var proposal = await _context.JobProposals.FindAsync(id);
            if (proposal == null) return NotFound();

            proposal.Status = status; // e.g., "Accepted" or "Rejected"
            
            // Logic: If Accepted, we can automatically create an entry in the Orders table here.
            
            await _context.SaveChangesAsync();
            return Ok(new { message = $"Proposal marked as {status}" });
        }
    }
}