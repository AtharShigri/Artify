using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Artify.Api.Repositories.Interfaces;
using Artify.Api.Services.Interfaces;
using System;
using System.Threading.Tasks;

namespace Artify.Api.Controllers.Shared
{
    [Route("api/chat")]
    [ApiController]
    [Authorize]
    public class ChatController : ControllerBase
    {
        private readonly IChatService _chatService;
        private readonly IBuyerRepository _buyerRepo;

        public ChatController(IChatService chatService, IBuyerRepository buyerRepo)
        {
            _chatService = chatService;
            _buyerRepo = buyerRepo;
        }

        [HttpGet("conversations")]
        public async Task<IActionResult> GetConversations()
        {
            try
            {
                var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(userIdString) || !Guid.TryParse(userIdString, out var userId))
                {
                    return Unauthorized(new { message = "Invalid user token." });
                }

                var conversations = await _chatService.GetUserConversationsAsync(userId);
                return Ok(conversations);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpGet("history/{conversationId}")]
        public async Task<IActionResult> GetChatHistory(Guid conversationId)
        {
            try
            {
                var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(userIdString) || !Guid.TryParse(userIdString, out var userId))
                {
                    return Unauthorized(new { message = "Invalid user token." });
                }

                // Security Check: Verify user is a participant
                if (!await _chatService.IsUserInConversationAsync(conversationId, userId))
                {
                    return Forbid("You do not have permission to view this chat history.");
                }

                var history = await _chatService.GetChatHistoryAsync(conversationId);
                return Ok(history);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpPost("start/{artistProfileId}")]
        public async Task<IActionResult> StartConversation(Guid artistProfileId)
        {
            try
            {
                var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(userIdString) || !Guid.TryParse(userIdString, out var userId))
                {
                    return Unauthorized(new { message = "Invalid user token." });
                }

                // Resolve the User ID from the Artist Profile ID
                var artistProfile = await _buyerRepo.GetArtistProfileByIdAsync(artistProfileId);
                if (artistProfile == null)
                {
                    return NotFound(new { message = "Artist profile not found." });
                }

                var sellerId = artistProfile.UserId;

                var conversation = await _chatService.GetOrCreateConversationAsync(userId, sellerId);
                return Ok(conversation);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpDelete("{conversationId}")]
        public async Task<IActionResult> DeleteChat(Guid conversationId)
        {
            try
            {
                var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(userIdString) || !Guid.TryParse(userIdString, out var userId))
                {
                    return Unauthorized(new { message = "Invalid user token." });
                }

                var deleted = await _chatService.DeleteConversationAsync(conversationId, userId);
                if (!deleted)
                {
                    return Forbid("You do not have permission to delete this chat or it does not exist.");
                }

                return Ok(new { message = "Chat deleted successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }
    }
}
