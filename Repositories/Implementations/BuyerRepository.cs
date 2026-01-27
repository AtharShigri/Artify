using Artify.Api.Data;
using Artify.Api.Models;
using Artify.Api.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Artify.Api.Repositories.Implementations
{
    public class BuyerRepository : BaseRepository, IBuyerRepository
    {
        public BuyerRepository(ApplicationDbContext context) : base(context) { }

        // Buyer Profile
        public async Task<ApplicationUser?> GetBuyerByIdAsync(Guid buyerId)
        {
            //to fix type error. Guid to String
            var buyerIdString = buyerId;

            return await _context.Users
                .FirstOrDefaultAsync(u => u.Id == buyerIdString);
        }

        public async Task<ApplicationUser?> GetBuyerByEmailAsync(string email)
        {
            return await _context.Users
                .FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<bool> UpdateBuyerAsync(ApplicationUser buyer)
        {
            _context.Users.Update(buyer);
            return await SaveAsync();
        }

        public async Task<bool> DeleteBuyerAsync(Guid buyerId)
        {
            var user = await _context.Users.FindAsync(buyerId);
            if (user == null) return false;

            _context.Users.Remove(user);
            return await SaveAsync();
        }

        // Marketplace
        public async Task<IEnumerable<Artwork>> GetFeaturedArtworksAsync(int count = 10)
        {
            return await _context.Artworks
                .Where(a => a.IsForSale && a.Stock > 0)
                .OrderByDescending(a => a.LikesCount)
                .Take(count)
                .Include(a => a.ArtistProfile)
                .ThenInclude(ap => ap.User)
                .ToListAsync();
        }

        public async Task<Artwork?> GetArtworkByIdAsync(Guid artworkId)
        {
            return await _context.Artworks
                .Include(a => a.ArtistProfile)
                .ThenInclude(ap => ap.User)
                .FirstOrDefaultAsync(a => a.ArtworkId == artworkId);
        }

        public async Task<IEnumerable<Artwork>> GetArtworksByCategoryAsync(Category? category, int page = 1, int pageSize = 20)
        {
            var query = _context.Artworks
                .Where(a => a.IsForSale && a.Stock > 0);

            if (category != null)
            {
                query = query.Where(a => a.CategoryEntity == category);
            }

            return await query
                .OrderByDescending(a => a.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Include(a => a.ArtistProfile)
                    .ThenInclude(ap => ap.User)
                .ToListAsync();
        }

        public async Task<IEnumerable<Artwork>> SearchArtworksAsync(string query, decimal? minPrice = null, decimal? maxPrice = null, string sortBy = "newest")
        {
            var artworksQuery = _context.Artworks.AsQueryable();

            if (!string.IsNullOrWhiteSpace(query))
            {
                artworksQuery = artworksQuery.Where(a =>
                    a.Title.Contains(query) ||
                    a.Description.Contains(query));
            }

            if (minPrice.HasValue)
            {
                artworksQuery = artworksQuery.Where(a => a.Price >= minPrice.Value);
            }

            if (maxPrice.HasValue)
            {
                artworksQuery = artworksQuery.Where(a => a.Price <= maxPrice.Value);
            }

            artworksQuery = sortBy.ToLower() switch
            {
                "price_asc" => artworksQuery.OrderBy(a => a.Price),
                "price_desc" => artworksQuery.OrderByDescending(a => a.Price),
                "popular" => artworksQuery.OrderByDescending(a => a.LikesCount),
                _ => artworksQuery.OrderByDescending(a => a.CreatedAt)
            };

            return await artworksQuery
                .Where(a => a.IsForSale && a.Stock > 0)
                .Include(a => a.ArtistProfile)
                .ThenInclude(ap => ap.User)
                .ToListAsync();
        }

        public async Task<ArtistProfile?> GetArtistProfileByIdAsync(Guid artistProfileId)
        {
            return await _context.ArtistProfiles
                .Include(ap => ap.User)
                .Include(ap => ap.Artworks)
                .FirstOrDefaultAsync(ap => ap.ArtistProfileId == artistProfileId);
        }

        public async Task<IEnumerable<ArtistProfile>> GetFeaturedArtistsAsync(int count = 10)
        {
            return await _context.ArtistProfiles
                .OrderByDescending(ap => ap.Rating)
                .Take(count)
                .Include(ap => ap.User)
                .ToListAsync();
        }

        public async Task<IEnumerable<ArtistProfile>> GetAllArtistsAsync(int page = 1, int pageSize = 20)
        {
            return await _context.ArtistProfiles
                .OrderBy(ap => ap.User.FullName)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Include(ap => ap.User)
                .Include(ap => ap.Artworks)
                .ToListAsync();
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await SaveAsync();
        }

        public async Task<bool> IncrementArtworkViewsAsync(Guid artworkId)
        {
            var artwork = await _context.Artworks.FindAsync(artworkId);
            if (artwork == null) return false;

            artwork.ViewsCount++;
            _context.Artworks.Update(artwork);
            return await SaveAsync();
        }
    }
}