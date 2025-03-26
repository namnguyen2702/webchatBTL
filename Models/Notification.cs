using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using webchatBTL.Models;

#nullable disable

namespace webchatBTL.Models
{
    public partial class Notification
    {
        [Key]
        public int NotificationId { get; set; }
        public int UserId { get; set; }
        [Required]
        [StringLength(255)]
        public string Message { get; set; }
        public bool IsRead { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime CreatedAt { get; set; }

        [ForeignKey(nameof(UserId))]
        [InverseProperty("Notifications")]
        public virtual User User { get; set; }
    }
}
