using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace BTL.Models
{
    public partial class SubscriptionPlan
    {
        public SubscriptionPlan()
        {
            CompanySubscriptions = new HashSet<CompanySubscription>();
        }

        [Key]
        public int PlanId { get; set; }
        [Required]
        [StringLength(50)]
        public string PlanName { get; set; }
        public int? MaxUsers { get; set; }
        public int? MaxStorage { get; set; }
        [Column(TypeName = "decimal(10, 2)")]
        public decimal Price { get; set; }

        [InverseProperty(nameof(CompanySubscription.Plan))]
        public virtual ICollection<CompanySubscription> CompanySubscriptions { get; set; }
    }
}
