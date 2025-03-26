using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using webchatBTL.Models;

#nullable disable

namespace webchatBTL.Models
{
    public partial class Company
    {
        public Company()
        {
            Invoices = new HashSet<Invoice>();
            Projects = new HashSet<Project>();
            Users = new HashSet<User>();
            CompanySubscriptions = new HashSet<CompanySubscription>();
        }

        [Key]
        public int CompanyId { get; set; }
        [Required]
        [StringLength(100)]
        public string CompanyName { get; set; }
        [Required]
        [StringLength(100)]
        public string Domain { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime CreatedAt { get; set; }

        [InverseProperty("Company")]
        public virtual CompanySetting CompanySetting { get; set; }
        [InverseProperty("Company")]
        // public virtual CompanySubscription CompanySubscription { get; set; }
		public virtual ICollection<CompanySubscription> CompanySubscriptions { get; set; }


		[InverseProperty(nameof(Invoice.Company))]
        public virtual ICollection<Invoice> Invoices { get; set; }
        [InverseProperty(nameof(Project.Company))]
        public virtual ICollection<Project> Projects { get; set; }
        [InverseProperty(nameof(User.Company))]
        public virtual ICollection<User> Users { get; set; }
    }
}
