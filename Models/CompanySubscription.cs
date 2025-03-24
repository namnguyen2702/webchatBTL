using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace BTL.Models
{
    public partial class CompanySubscription
    {
        [Key]
        public int CompanyId { get; set; }
        public int PlanId { get; set; }
        [Column(TypeName = "date")]
        public DateTime StartDate { get; set; }
        [Column(TypeName = "date")]
        public DateTime? EndDate { get; set; }

        [ForeignKey(nameof(CompanyId))]
        [InverseProperty("CompanySubscription")]
        public virtual Company Company { get; set; }
        [ForeignKey(nameof(PlanId))]
        [InverseProperty(nameof(SubscriptionPlan.CompanySubscriptions))]
        public virtual SubscriptionPlan Plan { get; set; }
    }
}
