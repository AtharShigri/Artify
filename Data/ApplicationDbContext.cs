using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Artify.Api.Models;

namespace Artify.Api.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // 🎨 Core Entities
        public DbSet<ArtistProfile> ArtistProfiles { get; set; }
        public DbSet<Artwork> Artworks { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Review> Reviews { get; set; }

        // 💳 Supporting / Advanced Entities
        public DbSet<AIHashRecord> AIHashRecords { get; set; }
        public DbSet<TransactionLog> TransactionLogs { get; set; }
        public DbSet<AdminActivity> AdminActivities { get; set; }

        // 🧭 Optional: Configure relationships & constraints
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Example: One-to-one between Artwork and AIHashRecord
            builder.Entity<Artwork>()
                .HasOne(a => a.ArtistProfile)
                .WithMany(ap => ap.Artworks)
                .HasForeignKey(a => a.ArtistProfileId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<AIHashRecord>()
                .HasOne(h => h.Artwork)
                .WithOne()
                .HasForeignKey<AIHashRecord>(h => h.ArtworkId)
                .OnDelete(DeleteBehavior.Cascade);
     

            builder.Entity<Order>()
                .HasOne(o => o.ArtistProfile)
                .WithMany(ap => ap.Orders)
                .HasForeignKey(o => o.ArtistProfileId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Order>()
                .HasOne(o => o.Buyer)
                .WithMany()
                .HasForeignKey(o => o.BuyerId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<TransactionLog>()
                .HasOne(t => t.Order)
                .WithOne()
                .HasForeignKey<TransactionLog>(t => t.OrderId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
