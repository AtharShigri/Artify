using System;

namespace artifi.Api.DTOs.Shared
{
    public class ConversationDto
    {
        public Guid Id { get; set; }
        public Guid ParticipantA_Id { get; set; }
        public Guid ParticipantB_Id { get; set; }
        public string ParticipantA_Name { get; set; }
        public string ParticipantB_Name { get; set; }
        public DateTime CreatedAt { get; set; }
        public string LastMessage { get; set; }
    }
}