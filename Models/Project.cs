using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using webchatBTL.Models;

#nullable disable

namespace webchatBTL.Models
{
    public partial class Project
    {
        public Project()
        {
            Files = new HashSet<File>();
            Tasks = new HashSet<Task>();
        }

        [Key]
        public int ProjectId { get; set; }
        [Required]
        [StringLength(100)]
        public string ProjectName { get; set; }
        [StringLength(255)]
        public string Description { get; set; }
        [Column(TypeName = "date")]
        public DateTime? StartDate { get; set; }
        [Column(TypeName = "date")]
        public DateTime? EndDate { get; set; }
        public int CompanyId { get; set; }
        public int CreatedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime CreatedAt { get; set; }
        [Required]
        [StringLength(50)]
        public string Status { get; set; }

        [ForeignKey(nameof(CompanyId))]
        [InverseProperty("Projects")]
        public virtual Company Company { get; set; }
        [ForeignKey(nameof(CreatedBy))]
        [InverseProperty(nameof(User.Projects))]
        public virtual User CreatedByNavigation { get; set; }
        [InverseProperty(nameof(File.Project))]
        public virtual ICollection<File> Files { get; set; }
        [InverseProperty(nameof(Task.Project))]
        public virtual ICollection<Task> Tasks { get; set; }
    }
}
