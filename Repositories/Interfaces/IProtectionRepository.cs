using Artify.Api.Models;

namespace Artify.Api.Repositories.Interfaces
{
    public interface IProtectionRepository
    {
        // Hash
        Task AddHashRecordAsync(ArtworkHash hash);
        Task<ArtworkHash?> GetArtworkHashAsync(Guid artworkId);
        Task<IEnumerable<ArtworkHash>> GetAllHashesAsync();

        // Metadata
        Task AddMetadataLogAsync(ArtworkMetadataLog log);
        Task<ArtworkMetadataLog?> GetMetadataAsync(Guid artworkId);

        // Plagiarism
        Task AddPlagiarismLogAsync(PlagiarismLog log);
    }
}
