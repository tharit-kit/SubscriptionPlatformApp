using SubscriptionPlatformApp.Domain.Entities.Shared;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SubscriptionPlatformApp.Domain.Entities
{
    [Table("Subscriptions")]
    public class Subscriptions : BaseEntity
    {
        [Key]
        [Column("SubscriptionId")]
        public Guid SubscriptionId { get; set; }

        [Required]
        [Column("TenantId")]
        public Guid TenantId { get; set; }

        [Required]
        [Column("SubscriptionType")]
        public required string SubscriptionType { get; set; }

        [Required]
        [Column("SubscriptionStatus")]
        public required string SubscriptionStatus { get; set; }

        [Required]
        [Column("SubscribedFrom")]
        public DateTime SubscribedFrom { get; set; }

        [Column("SubscribedTo")]
        public DateTime? SubscribedTo { get; set; }

        public Tenants Tenant { get; set; } = null!;
        public ICollection<Payments> Payments { get; set; } = new List<Payments>();
    }
}
