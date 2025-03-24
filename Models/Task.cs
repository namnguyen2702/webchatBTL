using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace webchatBTL.Models
{
    public partial class Task
    {
        [Key]
        public int TaskId { get; set; }
        [Required]
        [StringLength(100)]
        public string TaskName { get; set; }
        [StringLength(255)]
        public string Description { get; set; }
        public int ProjectId { get; set; }
        public int? AssignedTo { get; set; }
        [Required]
        [StringLength(50)]
        public string Priority { get; set; }
        [Column(TypeName = "date")]
        public DateTime? DueDate { get; set; }
        [Required]
        [StringLength(50)]
        public string Status { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime CreatedAt { get; set; }

        [ForeignKey(nameof(AssignedTo))]
        [InverseProperty(nameof(User.Tasks))]
        public virtual User AssignedToNavigation { get; set; }
        [ForeignKey(nameof(ProjectId))]
        [InverseProperty("Tasks")]
        public virtual Project Project { get; set; }
    }
}
