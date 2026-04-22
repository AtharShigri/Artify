using Artify.Api.Models;
using Artify.Api.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;
using Artify.Api.Data;


namespace Artify.Api.Repositories.Implementations
{
    public class ChatRepository : IChatRepository
    {
        private readonly ApplicationDbContext _context;

        public ChatRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Conversation> GetConversationByIdAsync(Guid id)
        {
            return await _context.Conversations
                .Include(c => c.Messages)
                .Include(c => c.ParticipantA)
                .Include(c => c.ParticipantB)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<IEnumerable<Conversation>> GetUserConversationsAsync(Guid userId)
        {
            return await _context.Conversations
                .Include(c => c.ParticipantA)
                .Include(c => c.ParticipantB)
                .Include(c => c.Messages)
                .Where(c => c.ParticipantA_Id == userId || c.ParticipantB_Id == userId)
                .OrderByDescending(c => c.CreatedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<ChatMessage>> GetMessageHistoryAsync(Guid conversationId)
        {
            return await _context.ChatMessages
                .Include(m => m.Sender)
                .Where(m => m.ConversationId == conversationId)
                .OrderBy(m => m.Timestamp)
                .ToListAsync();
        }

        public async Task AddMessageAsync(ChatMessage message)
        {
            await _context.ChatMessages.AddAsync(message);
        }

        public async Task<Conversation> GetOrCreateConversationAsync(Guid participantAId, Guid participantBId)
        {
            var conversation = await _context.Conversations
                .FirstOrDefaultAsync(c => 
                    (c.ParticipantA_Id == participantAId && c.ParticipantB_Id == participantBId) ||
                    (c.ParticipantA_Id == participantBId && c.ParticipantB_Id == participantAId));

            if (conversation == null)
            {
                conversation = new Conversation
                {
                    ParticipantA_Id = participantAId,
                    ParticipantB_Id = participantBId,
                    CreatedAt = DateTime.UtcNow
                };
                await _context.Conversations.AddAsync(conversation);
                await _context.SaveChangesAsync();
            }

            return conversation;
        }

        public async Task<bool> IsUserParticipantAsync(Guid conversationId, Guid userId)
        {
            return await _context.Conversations
                .AnyAsync(c => c.Id == conversationId && (c.ParticipantA_Id == userId || c.ParticipantB_Id == userId));
        }

        public async Task<bool> DeleteConversationAsync(Guid id)
        {
            var conversation = await _context.Conversations
                .Include(c => c.Messages)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (conversation == null) return false;

            _context.ChatMessages.RemoveRange(conversation.Messages);
            _context.Conversations.Remove(conversation);
            return true;
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}