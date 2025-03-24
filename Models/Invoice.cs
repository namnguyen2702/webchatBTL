using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace BTL.Models
{
    public partial class Invoice
    {
        [Key]
        public int InvoiceId { get; set; }
        public int CompanyId { get; set; }
        [Column(TypeName = "decimal(10, 2)")]
        public decimal Amount { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime PaymentDate { get; set; }
        [StringLength(50)]
        public string Status { get; set; }

        [ForeignKey(nameof(CompanyId))]
        [InverseProperty("Invoices")]
        public virtual Company Company { get; set; }
    }
}
