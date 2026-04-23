using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Artify.Api.Data;
using Artify.Api.Models;
using Artify.Api.DTOs.Shared;
using System.Security.Claims;

namespace Artify.Api.Controllers.Agency
{
    [Route("api/agency")]
    [ApiController]
    [Authorize]
    public class AgencyController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public AgencyController(ApplicationDbContext context)
        {
            _context = context;
        }

        // 1. Convert Individual to Agency or Update Agency Info
        [HttpPost("setup")]
        public async Task<IActionResult> SetupAgency([FromBody] AgencyDto dto)
        {
            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            
            // Check if user already owns an agency
            var existingAgency = await _context.Agencies.FirstOrDefaultAsync(a => a.OwnerId == userId);
            
            if (existingAgency != null)
            {
                existingAgency.Name = dto.Name;
                existingAgency.Description = dto.Description;
                _context.Agencies.Update(existingAgency);
            }
            else
            {
                var agency = new Artify.Api.Models.Agency
                {
                    Id = Guid.NewGuid(),
                    OwnerId = userId,
                    Name = dto.Name,
                    Description = dto.Description
                };
                _context.Agencies.Add(agency);
            }

            await _context.SaveChangesAsync();
            return Ok(new { message = "Agency profile updated successfully" });
        }

        // 2. Add a Member to the Agency (Artist as Agency logic)
        [HttpPost("members")]
        public async Task<IActionResult> AddMember([FromBody] string memberEmail)
        {
            var ownerId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var agency = await _context.Agencies.FirstOrDefaultAsync(a => a.OwnerId == ownerId);
            
            if (agency == null) return BadRequest("You do not own an agency.");

            var newMember = await _context.Users.FirstOrDefaultAsync(u => u.Email == memberEmail);
            if (newMember == null) return NotFound("User not found.");

            var membership = new AgencyMember
            {
                AgencyId = agency.Id,
                UserId = newMember.Id,
                RoleInAgency = "Member" 
            };

            _context.AgencyMembers.Add(membership);
            await _context.SaveChangesAsync();

            return Ok(new { message = $"{newMember.FullName} added to your agency." });
        }

        // 3. Get Agency Details & Team
        [HttpGet("my-team")]
        public async Task<IActionResult> GetMyTeam()
        {
            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var agency = await _context.Agencies
                .Include(a => a.Members)
                .ThenInclude(m => m.User)
                .FirstOrDefaultAsync(a => a.OwnerId == userId);

            if (agency == null) return NotFound("Agency not found.");

            return Ok(new {
                AgencyName = agency.Name,
                Members = agency.Members.Select(m => new {
                    m.User.FullName,
                    m.User.Email,
                    m.RoleInAgency
                })
            });
        }
    }
}