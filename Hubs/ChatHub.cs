using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.Authorization;
using Artify.Api.Services.Interfaces;
using System.Security.Claims;

namespace Artify.Api.Hubs
{
    [Authorize]
    public class ChatHub : Hub
    {
        private readonly IChatService _chatService;

        public ChatHub(IChatService chatService)
        {
            _chatService = chatService;
        }

        // ── On connect: automatically join all of the user's conversation groups ─
        // This is the key to real-time delivery: the receiver is always in their
        // groups, even before they open the chat page.
        public override async Task OnConnectedAsync()
        {
            var userIdString = Context.UserIdentifier;
            if (!string.IsNullOrEmpty(userIdString) && Guid.TryParse(userIdString, out var userId))
            {
                var conversations = await _chatService.GetUserConversationsAsync(userId);
                foreach (var conv in conversations)
                {
                    await Groups.AddToGroupAsync(Context.ConnectionId, conv.Id.ToString());
                }
            }
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            // SignalR automatically removes the connection from all groups on disconnect.
            await base.OnDisconnectedAsync(exception);
        }

        // ── Send a message ────────────────────────────────────────────────────────
        public async Task SendMessage(Guid conversationId, string content)
        {
            var userIdString = Context.UserIdentifier;
            if (string.IsNullOrEmpty(userIdString) || !Guid.TryParse(userIdString, out var senderId))
                throw new HubException("Unauthorized");

            // Security: must be a participant
            if (!await _chatService.IsUserInConversationAsync(conversationId, senderId))
                throw new HubException("Access Denied: you are not a participant in this conversation.");

            if (string.IsNullOrWhiteSpace(content))
                throw new HubException("Message cannot be empty.");

            // Save to DB, run moderation, send notification
            var messageDto = await _chatService.SaveAndProcessMessageAsync(conversationId, senderId, content);

            // Broadcast to everyone in the conversation group (both participants)
            await Clients.Group(conversationId.ToString()).SendAsync("ReceiveMessage", messageDto);

            // Warn the sender if the message was flagged
            if (messageDto.IsFlagged)
            {
                await Clients.Caller.SendAsync("ReceiveWarning",
                    "⚠️ Policy Alert: Sharing contact details or off-platform payment methods violates Artify's Terms of Service. Repeated violations may result in account suspension.");
            }
        }

        // ── Explicit join (called when user opens a conversation) ─────────────────
        // Still kept so newly created conversations get joined without reconnect.
        public async Task JoinChat(Guid conversationId)
        {
            var userIdString = Context.UserIdentifier;
            if (string.IsNullOrEmpty(userIdString) || !Guid.TryParse(userIdString, out var userId))
                throw new HubException("Unauthorized");

            if (!await _chatService.IsUserInConversationAsync(conversationId, userId))
                throw new HubException("Access Denied");

            await Groups.AddToGroupAsync(Context.ConnectionId, conversationId.ToString());
        }

        // ── Leave a conversation group ────────────────────────────────────────────
        public async Task LeaveChat(Guid conversationId)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, conversationId.ToString());
        }
    }
}