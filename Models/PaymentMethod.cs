using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Artify.Api.Models
{
    public class PayoutMethod
    {
        [Key]
        public int Id { get; set; }

        // FK to AspNetUsers (Guid PK)
        [Required]
        public Guid ArtistId { get; set; }
        [ForeignKey("ArtistId")]
        public virtual ApplicationUser Artist { get; set; }

        [Required]
        public string Provider { get; set; } // "Easypaisa" or "JazzCash"

        [Required]
        [Phone]
        public string AccountNumber { get; set; } // Their mobile number

        public string? AccountName { get; set; }
    }
}