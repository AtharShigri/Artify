using System;

namespace Artify.Api.DTOs.Shared
{
    public class MessageDto
    {
        public int Id { get; set; }
        public int ConversationId { get; set; }
        public string SenderId { get; set; }
        public string Content { get; set; }
        public DateTime Timestamp { get; set; }
        public bool IsRead { get; set; }
        public bool IsFlagged { get; set; }
        public string FlagReason { get; set; }
    }
}