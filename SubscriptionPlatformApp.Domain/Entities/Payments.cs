using SubscriptionPlatformApp.Domain.Entities.Shared;
using SubscriptionPlatformApp.Domain.Enums;
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

        [Column("UserId")]
        public Guid? UserId { get; set; }

        [Required]
        [Column("SubscriptionId")]
        public Guid SubscriptionId { get; set; }

        [Column("Total")]
        public decimal? Total { get; set; }

        [Column("PaymentDate")]
        public DateTime? PaymentDate { get; set; }

        [Required]
        [Column("PaymentStatus")]
        public required PaymentStatus PaymentStatus { get; set; }

        public Tenants Tenant { get; set; } = null!; 
        public Subscriptions Subscription { get; set; } = null!;
        public Users? User { get; set; }
    }
}
