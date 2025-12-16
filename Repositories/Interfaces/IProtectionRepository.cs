using Artify.Api.Models;

namespace Artify.Api.Repositories.Interfaces
{
    public interface IProtectionRepository
    {
        Task AddHashRecordAsync(ArtworkHash hash);
        Task AddMetadataLogAsync(ArtworkMetadataLog log);
        Task AddPlagiarismLogAsync(PlagiarismLog log);
    }
}
