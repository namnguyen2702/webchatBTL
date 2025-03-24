using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace BTL.Models
{
    [Table("GroupInCompany")]
    public partial class GroupInCompany
    {
        public GroupInCompany()
        {
            Events = new HashSet<Event>();
            Users = new HashSet<User>();
        }

        [Key]
        public int GroupId { get; set; }
        [Required]
        [StringLength(255)]
        public string GroupName { get; set; }
        [StringLength(500)]
        public string Description { get; set; }
        public int ManagerId { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreatedAt { get; set; }

        [ForeignKey(nameof(ManagerId))]
        [InverseProperty(nameof(User.GroupInCompanies))]
        public virtual User Manager { get; set; }
        [InverseProperty(nameof(Event.Group))]
        public virtual ICollection<Event> Events { get; set; }
        [InverseProperty(nameof(User.Group))]
        public virtual ICollection<User> Users { get; set; }
    }
}
