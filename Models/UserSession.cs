using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace BTL.Models
{
    public partial class UserSession
    {
        [Key]
        public int SessionId { get; set; }
        public int UserId { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime LoginTime { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? LogoutTime { get; set; }
        [Required]
        public bool? IsActive { get; set; }

        [ForeignKey(nameof(UserId))]
        [InverseProperty("UserSessions")]
        public virtual User User { get; set; }
    }
}
