using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace webchatBTL.Models
{
    public partial class CompanySubscription
{
    [Key]
    public int Id { get; set; }  // Khóa chính độc lập

    public int CompanyId { get; set; }
    public int PlanId { get; set; }

    [Column(TypeName = "date")]
    public DateTime StartDate { get; set; }

    [Column(TypeName = "date")]
    public DateTime? EndDate { get; set; }

    [ForeignKey(nameof(CompanyId))]
    [InverseProperty("CompanySubscriptions")]
    public virtual Company Company { get; set; }

    [ForeignKey(nameof(PlanId))]
    [InverseProperty(nameof(SubscriptionPlan.CompanySubscriptions))]
    public virtual SubscriptionPlan Plan { get; set; }
}

}
