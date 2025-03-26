using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using webchatBTL.Models;

#nullable disable

namespace webchatBTL.Models
{
    public partial class Participant
    {
        [Key]
        public int ParticipantId { get; set; }
        public int EventId { get; set; }
        public int UserId { get; set; }
        public bool? IsConfirmed { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? JoinedAt { get; set; }

        [ForeignKey(nameof(EventId))]
        [InverseProperty("Participants")]
        public virtual Event Event { get; set; }
        [ForeignKey(nameof(UserId))]
        [InverseProperty("Participants")]
        public virtual User User { get; set; }
    }
}
