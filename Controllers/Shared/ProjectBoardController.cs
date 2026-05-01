using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using artifi.Api.Data;
using artifi.Api.Models;
using artifi.Api.DTOs.Shared;
using artifi.Api.Services.Interfaces;
using System.Security.Claims;

namespace artifi.Api.Controllers.Shared
{
    [Route("api/projectboard")]
    [ApiController]
    [Authorize]
    public class ProjectBoardController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly INotificationService _notificationService;
        private readonly IChatService _chatService;

        public ProjectBoardController(
            ApplicationDbContext context,
            INotificationService notificationService,
            IChatService chatService)
        {
            _context = context;
            _notificationService = notificationService;
            _chatService = chatService;
        }

        // ─── Helper ──────────────────────────────────────────────────────────
        private Guid GetUserId() => Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        // ─── BUYER SECTION ───────────────────────────────────────────────────

        [HttpPost("post-job")]
        [Authorize(Roles = "Buyer,Admin")]
        public async Task<IActionResult> PostJob([FromBody] JobPostDto dto)
        {
            var userId = GetUserId();

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
            var userId = GetUserId();

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

        // ─── ARTIST SECTION ──────────────────────────────────────────────────

        [HttpGet("browse")]
        [AllowAnonymous]
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
            var artistId = GetUserId();

            // Prevent duplicate applications
            var existingProposal = await _context.JobProposals
                .FirstOrDefaultAsync(p => p.JobPostId == dto.JobPostId && p.ApplicantId == artistId);

            if (existingProposal != null)
                return BadRequest(new { message = "You have already applied for this job." });

            var proposal = new JobProposal
            {
                Id = Guid.NewGuid(),
                JobPostId = dto.JobPostId,
                ApplicantId = artistId,
                CoverLetter = dto.CoverLetter,
                BidAmount = dto.BidAmount,
                Status = "Pending"
            };

            _context.JobProposals.Add(proposal);
            await _context.SaveChangesAsync();

            // ── Notify the buyer that a new proposal arrived ─────────────────
            var job = await _context.JobPosts
                .Include(j => j.Poster)
                .FirstOrDefaultAsync(j => j.Id == dto.JobPostId);

            if (job != null)
            {
                var artist = await _context.Users.FindAsync(artistId);
                var artistName = artist?.FullName ?? "An artist";

                await _notificationService.SendNotificationAsync(
                    job.PosterId,
                    "New Proposal Received",
                    $"{artistName} submitted a proposal for your project \"{job.Title}\" with a bid of PKR {dto.BidAmount:N0}.",
                    "Proposal",
                    $"/dashboard/buyer"
                );
            }

            return Ok(new { message = "Proposal submitted successfully" });
        }

        // ─── MANAGEMENT SECTION ──────────────────────────────────────────────

        [HttpPatch("proposals/{id}/status")]
        [Authorize(Roles = "Buyer,Admin")]
        public async Task<IActionResult> UpdateProposalStatus(Guid id, [FromBody] string status)
        {
            // Load proposal with all relations needed for notifications
            var proposal = await _context.JobProposals
                .Include(p => p.Applicant)
                .Include(p => p.JobPost)
                    .ThenInclude(j => j.Poster)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (proposal == null) return NotFound();

            // Verify caller is the poster
            var callerId = GetUserId();
            if (proposal.JobPost.PosterId != callerId)
                return Forbid();

            proposal.Status = status;
            await _context.SaveChangesAsync();

            // ── Notify the artist of the outcome ─────────────────────────────
            var artistId = proposal.ApplicantId;
            var buyerName = proposal.JobPost.Poster?.FullName ?? "The client";
            var jobTitle = proposal.JobPost.Title;

            if (status == "Accepted")
            {
                // 1. Notify artist
                await _notificationService.SendNotificationAsync(
                    artistId,
                    "🎉 Proposal Accepted!",
                    $"{buyerName} accepted your proposal for \"{jobTitle}\". A conversation has been started.",
                    "ProposalAccepted",
                    "/chat"
                );

                // 2. Notify buyer (confirm action)
                await _notificationService.SendNotificationAsync(
                    callerId,
                    "Proposal Accepted",
                    $"You accepted the proposal from {proposal.Applicant?.FullName ?? "the artist"} for \"{jobTitle}\".",
                    "ProposalAccepted",
                    "/chat"
                );

                // 3. Open/get conversation and send automatic welcome message
                try
                {
                    var conversation = await _chatService.GetOrCreateConversationAsync(callerId, artistId);

                    // Send system message as the buyer
                    await _chatService.SaveAndProcessMessageAsync(
                        conversation.Id,
                        callerId,
                        $"Hi! I've accepted your proposal for \"{jobTitle}\" 🎉 Looking forward to working with you. Please let me know when you're ready to get started."
                    );
                }
                catch (Exception ex)
                {
                    // Non-fatal — log and continue
                    Console.WriteLine($"[ProjectBoard] Auto-message failed: {ex.Message}");
                }
            }
            else if (status == "Rejected")
            {
                // Notify artist of rejection
                await _notificationService.SendNotificationAsync(
                    artistId,
                    "Proposal Not Selected",
                    $"Thank you for applying to \"{jobTitle}\". {buyerName} has decided to go with another artist this time. Keep applying!",
                    "ProposalRejected",
                    "/project-board"
                );
            }

            return Ok(new { message = $"Proposal marked as {status}" });
        }
    }
}