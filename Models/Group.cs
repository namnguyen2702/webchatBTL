using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using webchatBTL.Models;

#nullable disable

namespace webchatBTL.Models
{
    public partial class Group
    {
        public Group()
        {
            GroupMembers = new HashSet<GroupMember>();
        }

        [Key]
        public int GroupId { get; set; }
        [Required]
        [StringLength(100)]
        public string GroupName { get; set; }
        public int CreatedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime CreatedAt { get; set; }

        [ForeignKey(nameof(CreatedBy))]
        [InverseProperty(nameof(User.Groups))]
        public virtual User CreatedByNavigation { get; set; }
        [InverseProperty(nameof(GroupMember.Group))]
        public virtual ICollection<GroupMember> GroupMembers { get; set; }
    }
}
