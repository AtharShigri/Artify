using Artify.Api.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Artify.Api.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, IdentityRole<Guid>, Guid>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Artwork> Artworks { get; set; }
        public DbSet<ArtistProfile> ArtistProfiles { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Review> Reviews { get; set; }

        public DbSet<TransactionLog> TransactionLogs { get; set; }
        public DbSet<AdminActivity> AdminActivities { get; set; }
        public DbSet<AIHashRecord> AIHashRecords { get; set; }

        public DbSet<PlagiarismLog> PlagiarismLogs { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<ArtworkTag> ArtworkTags { get; set; }
        public DbSet<HiringRequest> HiringRequests { get; set; }
        public DbSet<Agency> Agencies { get; set; }
        public DbSet<AgencyMember> AgencyMembers { get; set; }
        public DbSet<JobPost> JobPosts { get; set; }
        public DbSet<JobProposal> JobProposals { get; set; }

        // New models
        public DbSet<Conversation> Conversations { get; set; }
        public DbSet<ChatMessage> ChatMessages { get; set; }
        public DbSet<EscrowTransaction> EscrowTransactions { get; set; }
        public DbSet<PayoutMethod> PayoutMethods { get; set; }
        public DbSet<Notification> Notifications { get; set; }

        // Protection models
        public DbSet<ArtworkHash> ArtworkHashes { get; set; }
        public DbSet<ArtworkMetadataLog> ArtworkMetadataLogs { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Notification -> User
            builder.Entity<Notification>()
                .HasOne(n => n.User)
                .WithMany()
                .HasForeignKey(n => n.UserId)
                .OnDelete(DeleteBehavior.Cascade);

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
                .HasOne(p => p.OriginalArtwork)
                .WithMany(a => a.PlagiarismLogsAsOriginal)
                .HasForeignKey(p => p.ArtworkId)
                .OnDelete(DeleteBehavior.Restrict); 

            builder.Entity<PlagiarismLog>()
                .HasOne(p => p.SuspectedArtwork)
                .WithMany(a => a.PlagiarismLogsAsSuspect)
                .HasForeignKey(p => p.SuspectedArtworkId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Order>()
                .HasOne(o => o.Buyer)
                .WithMany()
                .HasForeignKey(o => o.BuyerId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<AgencyMember>()
                .HasKey(am => new { am.AgencyId, am.UserId });

            builder.Entity<AgencyMember>()
                .HasOne(am => am.User)
                .WithMany()
                .HasForeignKey(am => am.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<AgencyMember>()
                .HasOne(am => am.Agency)
                .WithMany(a => a.Members)
                .HasForeignKey(am => am.AgencyId)
                .OnDelete(DeleteBehavior.Cascade); 
        
            builder.Entity<JobPost>()
                .HasOne(j => j.Poster)
                .WithMany()
                .HasForeignKey(j => j.PosterId)
                .OnDelete(DeleteBehavior.Restrict);

            // Conversation: two separate FK navigation props to ApplicationUser
            builder.Entity<Conversation>()
                .HasOne(c => c.ParticipantA)
                .WithMany()
                .HasForeignKey(c => c.ParticipantA_Id)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Conversation>()
                .HasOne(c => c.ParticipantB)
                .WithMany()
                .HasForeignKey(c => c.ParticipantB_Id)
                .OnDelete(DeleteBehavior.Restrict);

            // ChatMessage -> Sender
            builder.Entity<ChatMessage>()
                .HasOne(m => m.Sender)
                .WithMany()
                .HasForeignKey(m => m.SenderId)
                .OnDelete(DeleteBehavior.Restrict);

            // EscrowTransaction -> Order (one-to-one)
            builder.Entity<EscrowTransaction>()
                .HasOne(e => e.Order)
                .WithMany()
                .HasForeignKey(e => e.OrderId)
                .OnDelete(DeleteBehavior.Restrict);

            // PayoutMethod -> Artist
            builder.Entity<PayoutMethod>()
                .HasOne(p => p.Artist)
                .WithMany()
                .HasForeignKey(p => p.ArtistId)
                .OnDelete(DeleteBehavior.Cascade);
            // ArtworkHash -> Artwork
            builder.Entity<ArtworkHash>()
                .HasOne(h => h.Artwork)
                .WithMany()
                .HasForeignKey(h => h.ArtworkId)
                .OnDelete(DeleteBehavior.Cascade);

            // ArtworkMetadataLog -> Artwork
            builder.Entity<ArtworkMetadataLog>()
                .HasOne(m => m.Artwork)
                .WithMany()
                .HasForeignKey(m => m.ArtworkId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}