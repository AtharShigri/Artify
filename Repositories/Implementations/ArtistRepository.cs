using System.Security.Claims;
using Artify.Api.Models;
using Artify.Api.Repositories.Interfaces;
using Artify.Api.Data;
using Microsoft.EntityFrameworkCore;

namespace Artify.Api.Repositories.Implementations
{
    public class ArtistRepository : IArtistRepository
{
    private readonly ApplicationDbContext _context;

    public ArtistRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public Guid GetArtistId(ClaimsPrincipal user)
    {
        var idClaim = user.FindFirst(ClaimTypes.NameIdentifier);
        if (idClaim != null && Guid.TryParse(idClaim.Value, out var userId))
        {
            return _context.ArtistProfiles
                .Where(p => p.UserId == userId)
                .Select(p => p.ArtistProfileId)
                .FirstOrDefault();
        }
        return Guid.Empty;
    }

    public async Task<ApplicationUser> GetByIdAsync(Guid userId)
    {
        return await _context.Users
            .Include(u => u.ArtistProfile)
            .FirstOrDefaultAsync(u => u.Id == userId);
    }

    public async Task<ApplicationUser> GetByEmailAsync(string email)
    {
        return await _context.Users
            .Include(u => u.ArtistProfile)
            .FirstOrDefaultAsync(u => u.Email == email);
    }

    public async Task AddAsync(ApplicationUser artist)
    {
        await _context.Users.AddAsync(artist);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(ApplicationUser artist)
    {
        _context.Users.Update(artist);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(ApplicationUser artist)
    {
        _context.Users.Remove(artist);
        await _context.SaveChangesAsync();
    }
}
}
