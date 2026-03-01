using SubscriptionPlatformApp.Domain.Entities.Shared;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SubscriptionPlatformApp.Domain.Entities
{
    [Table("Tenants")]
    public class Tenants : BaseEntity
    {
        [Key]
        [Column("TenantId")]
        public Guid TenantId { get; set; }

        [Required]
        [MaxLength(128)]
        [Column("TenantName")]
        public required string TenantName { get; set; }

        [Required]
        [Column("BusinessTypeId")]
        public Guid BusinessTypeId { get; set; }

        [Required]
        [MaxLength(128)]
        [Column("TenantStatus")]
        public required string TenantStatus { get; set; }

        [Required]
        [Column("TenantAddressId")]
        public Guid TenantAddressId { get; set; }

        [Column("BillingAddressId")]
        public Guid BillingAddressId { get; set; }

        [Required]
        [MaxLength(128)]
        [Column("Subdomain")]
        public required string Subdomain { get; set; }

        public ICollection<Users> Users { get; set; } = new List<Users>();
        public ICollection<Subscriptions> Subscriptions { get; set; } = new List<Subscriptions>();
        public ICollection<Payments> Payments { get; set; } = new List<Payments>();
    }
}
