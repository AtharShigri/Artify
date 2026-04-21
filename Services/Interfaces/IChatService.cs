using System.Collections.Generic;
using System.Threading.Tasks;
using Artify.Api.DTOs.Shared;

namespace Artify.Api.Services.Interfaces
{
    public interface IChatService
    {
        Task<MessageDto> SaveAndProcessMessageAsync(Guid conversationId, Guid senderId, string content);
        Task<IEnumerable<MessageDto>> GetChatHistoryAsync(Guid conversationId);
        Task<IEnumerable<ConversationDto>> GetUserConversationsAsync(Guid userId);
        Task<ConversationDto> GetOrCreateConversationAsync(Guid buyerId, Guid sellerId);
        Task<bool> IsUserInConversationAsync(Guid conversationId, Guid userId);
    }
}