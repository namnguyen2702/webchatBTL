using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace BTL.Models
{
    public partial class Event
    {
        public Event()
        {
            Participants = new HashSet<Participant>();
        }

        [Key]
        public int EventId { get; set; }
        public int UserId { get; set; }
        [Required]
        [StringLength(100)]
        public string Subject { get; set; }
        [StringLength(300)]
        public string Description { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime Start { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? End { get; set; }
        [StringLength(10)]
        public string ThemeColor { get; set; }
        public bool IsFullDay { get; set; }
        public int? GroupId { get; set; }

        [ForeignKey(nameof(GroupId))]
        [InverseProperty(nameof(GroupInCompany.Events))]
        public virtual GroupInCompany Group { get; set; }
        [ForeignKey(nameof(UserId))]
        [InverseProperty("Events")]
        public virtual User User { get; set; }
        [InverseProperty(nameof(Participant.Event))]
        public virtual ICollection<Participant> Participants { get; set; }
    }
}
