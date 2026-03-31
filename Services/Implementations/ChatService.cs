using Artify.Api.DTOs.Shared;
using Artify.Api.Models;
using Artify.Api.Repositories.Interfaces;
using Artify.Api.Services.Interfaces;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Artify.Api.Services.Implementations
{
    public class ChatService : IChatService
    {
        private readonly IChatRepository _chatRepo;
        private readonly IMapper _mapper;

        public ChatService(IChatRepository chatRepo, IMapper mapper)
        {
            _chatRepo = chatRepo;
            _mapper = mapper;
        }

        public async Task<MessageDto> SaveAndProcessMessageAsync(Guid conversationId, Guid senderId, string content)
        {
            // 1. Regex for Platform Leakage Detection (Phone numbers & WhatsApp)
            string pattern = @"(\+92|92|03)\d{2}-?\d{7}|whatsapp|contact me at|call me|email me|dm me|snapchat|instagram|facebook|twitter|tiktok|telegram|discord|skype|linkedin|direct pay|paypal|stripe|cash app|venmo|zelle|cash|Jazzcash|Easypaisa|Payoneer|Paytm";
            bool isFlagged = Regex.IsMatch(content, pattern, RegexOptions.IgnoreCase);

            var message = new ChatMessage
            {
                ConversationId = conversationId,
                SenderId = senderId,
                Content = content,
                Timestamp = DateTime.UtcNow,
                IsFlagged = isFlagged,
                FlagReason = isFlagged ? "Detected attempt to share contact info/off-platform payment." : null
            };

            await _chatRepo.AddMessageAsync(message);
            await _chatRepo.SaveChangesAsync();

            return _mapper.Map<MessageDto>(message);
        }

        public async Task<IEnumerable<MessageDto>> GetChatHistoryAsync(Guid conversationId)
        {
            var history = await _chatRepo.GetMessageHistoryAsync(conversationId);
            return _mapper.Map<IEnumerable<MessageDto>>(history);
        }

        public async Task<IEnumerable<ConversationDto>> GetUserConversationsAsync(Guid userId)
        {
            var conversations = await _chatRepo.GetUserConversationsAsync(userId);
            return _mapper.Map<IEnumerable<ConversationDto>>(conversations);
        }

        public async Task<ConversationDto> GetOrCreateConversationAsync(Guid buyerId, Guid sellerId)
        {
            var conversation = await _chatRepo.GetOrCreateConversationAsync(buyerId, sellerId);
            return _mapper.Map<ConversationDto>(conversation);
        }
    }
}