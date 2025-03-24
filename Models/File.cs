using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace BTL.Models
{
    public partial class File
    {
        [Key]
        public int FileId { get; set; }
        [Required]
        [StringLength(255)]
        public string FileName { get; set; }
        [Required]
        [StringLength(255)]
        public string FilePath { get; set; }
        public int UploadedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime UploadedAt { get; set; }
        public int? ProjectId { get; set; }
        public int? MessageId { get; set; }
        [StringLength(50)]
        public string FileType { get; set; }

        [ForeignKey(nameof(MessageId))]
        [InverseProperty("Files")]
        public virtual Message Message { get; set; }
        [ForeignKey(nameof(ProjectId))]
        [InverseProperty("Files")]
        public virtual Project Project { get; set; }
        [ForeignKey(nameof(UploadedBy))]
        [InverseProperty(nameof(User.Files))]
        public virtual User UploadedByNavigation { get; set; }
    }
}
