using Microsoft.AspNetCore.SignalR;
using Artify.Api.Services.Interfaces;
using Artify.Api.DTOs.Shared;
using System.Threading.Tasks;

namespace Artify.Api.Hubs
{
    public class ChatHub : Hub
    {
        private readonly IChatService _chatService;

        public ChatHub(IChatService chatService)
        {
            _chatService = chatService;
        }

        // Called from React when a user sends a message
        public async Task SendMessage(Guid conversationId, Guid senderId, string content)
        {
            // Process and save via Service (includes Regex flagging)
            var messageDto = await _chatService.SaveAndProcessMessageAsync(conversationId, senderId, content);

            // Broadcast to the group (ConversationId is the Group Name)
            await Clients.Group(conversationId.ToString()).SendAsync("ReceiveMessage", messageDto);

            // If flagged, send a private warning to the sender
            if (messageDto.IsFlagged)
            {
                await Clients.Caller.SendAsync("ReceiveWarning", "Security Alert: Sharing contact details or taking payments off-platform is a violation of Artify policy.");
            }
        }

        public async Task JoinChat(Guid conversationId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, conversationId.ToString());
        }

        public async Task LeaveChat(Guid conversationId)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, conversationId.ToString());
        }
    }
}