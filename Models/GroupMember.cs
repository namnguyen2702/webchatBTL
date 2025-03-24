using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace BTL.Models
{
    public partial class GroupMember
    {
        [Key]
        public int GroupId { get; set; }
        [Key]
        public int UserId { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime JoinedAt { get; set; }

        [ForeignKey(nameof(GroupId))]
        [InverseProperty("GroupMembers")]
        public virtual Group Group { get; set; }
        [ForeignKey(nameof(UserId))]
        [InverseProperty("GroupMembers")]
        public virtual User User { get; set; }
    }
}
