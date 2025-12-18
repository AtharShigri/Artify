// ========================= ApplicationDbContext.cs (Safe, Code-Only Version) =========================
using Artify.Api.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Artify.Api.Data
{
    public class ApplicationDbContext : IdentityDbContext<Artist>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // Existing models (already in DB)
        public DbSet<Artwork> Artworks { get; set; }
        public DbSet<ArtistProfile> ArtistProfiles { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<TransactionLog> TransactionLogs { get; set; }
        public DbSet<AdminActivity> AdminActivities { get; set; }
        public DbSet<AIHashRecord> AIHashRecords { get; set; }

        public DbSet<ApplicationUser> Users { get; set; }

        // ----------------- Safe code-only additions -----------------
        public DbSet<PlagiarismLog> PlagiarismLogs { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<ArtworkTag> ArtworkTags { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // ----------------- Safe relationship configurations -----------------

            // Many-to-many: Artwork <-> Tags
            builder.Entity<Artwork>()
                .HasMany(a => a.Tags)
                .WithMany(t => t.Artworks);

            // Category -> Artworks
            builder.Entity<Category>()
                .HasMany(c => c.Artworks)
                .WithOne(a => a.CategoryEntity)
                .OnDelete(DeleteBehavior.SetNull); 

            // Category -> Services
            builder.Entity<Category>()
                .HasMany(c => c.Services)
                .WithOne(s => s.CategoryEntity)
                .OnDelete(DeleteBehavior.SetNull); 

            // PlagiarismLog relationships
            builder.Entity<PlagiarismLog>()
                .HasOne(p => p.Artwork)
                .WithMany()
                .HasForeignKey(p => p.ArtworkId)
                .OnDelete(DeleteBehavior.Restrict); 

            builder.Entity<PlagiarismLog>()
                .HasOne(p => p.SuspectedArtwork)
                .WithMany()
                .HasForeignKey(p => p.SuspectedArtworkId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Order>()
        .HasOne(o => o.Buyer)
        .WithMany()
        .HasForeignKey(o => o.BuyerId)
        .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
