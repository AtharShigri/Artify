using artifi.Api.Models;

namespace artifi.Api.Repositories.Interfaces
{
    public interface IProtectionRepository
    {
        // Hash
        Task AddHashRecordAsync(ArtworkHash hash);
        Task UpdateHashRecordAsync(ArtworkHash hash);
        Task<ArtworkHash?> GetArtworkHashAsync(Guid artworkId);
        Task<IEnumerable<ArtworkHash>> GetAllHashesAsync();

        // Metadata
        Task AddMetadataLogAsync(ArtworkMetadataLog log);
        Task UpdateMetadataLogAsync(ArtworkMetadataLog log);
        Task<ArtworkMetadataLog?> GetMetadataAsync(Guid artworkId);

        // Plagiarism
        Task AddPlagiarismLogAsync(PlagiarismLog log);
    }
}
