using System;

namespace Artify.Api.DTOs.Shared
{
    public class ConversationDto
    {
        public int Id { get; set; }
        public string ParticipantA_Id { get; set; }
        public string ParticipantB_Id { get; set; }
        public string ParticipantA_Name { get; set; }
        public string ParticipantB_Name { get; set; }
        public DateTime CreatedAt { get; set; }
        public string LastMessage { get; set; }
    }
}