using System.Collections.Generic;
using System.Threading.Tasks;
using Artify.Api.Models;

namespace Artify.Api.Repositories.Interfaces
{
    public interface IChatRepository
    {
        Task<Conversation> GetConversationByIdAsync(Guid id);
        Task<IEnumerable<Conversation>> GetUserConversationsAsync(Guid userId);
        Task<IEnumerable<ChatMessage>> GetMessageHistoryAsync(Guid conversationId);
        Task AddMessageAsync(ChatMessage message);
        Task<Conversation> GetOrCreateConversationAsync(Guid participantAId, Guid participantBId);
        Task SaveChangesAsync();
    }
}