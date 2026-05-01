using System.Collections.Generic;
using System.Threading.Tasks;
using artifi.Api.Models;

namespace artifi.Api.Repositories.Interfaces
{
    public interface IChatRepository
    {
        Task<Conversation> GetConversationByIdAsync(Guid id);
        Task<IEnumerable<Conversation>> GetUserConversationsAsync(Guid userId);
        Task<IEnumerable<ChatMessage>> GetMessageHistoryAsync(Guid conversationId);
        Task AddMessageAsync(ChatMessage message);
        Task<Conversation> GetOrCreateConversationAsync(Guid participantAId, Guid participantBId);
        Task<bool> IsUserParticipantAsync(Guid conversationId, Guid userId);
        Task<bool> DeleteConversationAsync(Guid id);
        Task SaveChangesAsync();
    }
}