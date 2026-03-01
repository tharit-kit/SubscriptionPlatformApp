using SubscriptionPlatformApp.Domain.Entities.Shared;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SubscriptionPlatformApp.Domain.Entities
{
    [Table("Payments")]
    public class Payments : BaseEntity
    {
        [Key]
        [Column("PaymentId")]
        public Guid PaymentId { get; set; }

        [Required]
        [Column("TenantId")]
        public Guid TenantId { get; set; }
        
        [Required]
        [Column("SubscriptionId")]
        public Guid SubscriptionId { get; set; }

        [Column("Total")]
        public decimal? Total { get; set; }

        [Column("PaymentDate")]
        public DateTime? PaymentDate { get; set; }

        public PaymentStatus PaymentStatus { get; set; }

        public Tenants Tenant { get; set; } = null!; 
        public Subscription Subscription { get; set; } = null!;
    }
}
