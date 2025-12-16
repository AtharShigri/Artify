using Artify.Api.Models;
using Artify.Api.Repositories.Interfaces;

namespace Artify.Api.Repositories.Implementations
{
    public class ProtectionRepository : IProtectionRepository
    {
        public Task AddHashRecordAsync(ArtworkHash hash) => Task.CompletedTask;
        public Task AddMetadataLogAsync(ArtworkMetadataLog log) => Task.CompletedTask;
        public Task AddPlagiarismLogAsync(PlagiarismLog log) => Task.CompletedTask;
    }
}
