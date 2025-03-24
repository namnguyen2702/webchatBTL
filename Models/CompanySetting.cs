using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace BTL.Models
{
    public partial class CompanySetting
    {
        [Key]
        public int CompanyId { get; set; }
        [StringLength(50)]
        public string Theme { get; set; }
        [StringLength(255)]
        public string Logo { get; set; }

        [ForeignKey(nameof(CompanyId))]
        [InverseProperty("CompanySetting")]
        public virtual Company Company { get; set; }
    }
}
