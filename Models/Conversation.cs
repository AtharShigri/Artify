using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Artify.Api.Models
{
    public class Conversation
    {
        [Key]
        public Guid Id { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Participants — linked to AspNetUsers (Guid PK)
        public Guid ParticipantA_Id { get; set; }
        [ForeignKey("ParticipantA_Id")]
        public virtual ApplicationUser ParticipantA { get; set; }

        public Guid ParticipantB_Id { get; set; }
        [ForeignKey("ParticipantB_Id")]
        public virtual ApplicationUser ParticipantB { get; set; }

        public virtual ICollection<ChatMessage> Messages { get; set; } = new List<ChatMessage>();
    }
}