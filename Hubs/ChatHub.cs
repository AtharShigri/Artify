using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.Authorization;
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
        [Authorize]
        public async Task SendMessage(Guid conversationId, string content)
        {
            var userIdString = Context.UserIdentifier;
            if (string.IsNullOrEmpty(userIdString) || !Guid.TryParse(userIdString, out var senderId))
            {
                throw new HubException("Unauthorized");
            }

            // Security Check: Is the user a participant?
            if (!await _chatService.IsUserInConversationAsync(conversationId, senderId))
            {
                throw new HubException("Access Denied");
            }

            // Process and save
            var messageDto = await _chatService.SaveAndProcessMessageAsync(conversationId, senderId, content);

            // Broadcast to the group
            await Clients.Group(conversationId.ToString()).SendAsync("ReceiveMessage", messageDto);

            // Notify the other participant if they aren't in the group
            // (Handled by the logic that they should be listening to their user-specific group too if we wanted global alerts)

            if (messageDto.IsFlagged)
            {
                await Clients.Caller.SendAsync("ReceiveWarning", "Security Alert: Sharing contact details or taking payments off-platform is a violation of Artify policy.");
            }
        }

        [Authorize]
        public async Task JoinChat(Guid conversationId)
        {
            var userIdString = Context.UserIdentifier;
            if (string.IsNullOrEmpty(userIdString) || !Guid.TryParse(userIdString, out var userId))
            {
                throw new HubException("Unauthorized");
            }

            if (await _chatService.IsUserInConversationAsync(conversationId, userId))
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, conversationId.ToString());
            }
            else
            {
                throw new HubException("Access Denied");
            }
        }

        public async Task LeaveChat(Guid conversationId)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, conversationId.ToString());
        }
    }
}