using SubscriptionPlatformApp.Domain.Entities.Shared;
using SubscriptionPlatformApp.Domain.Enums;
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
        [MaxLength(64)]
        [Column("BusinessType")]
        public required string BusinessType { get; set; }

        [Required]
        [Column("TenantStatus")]
        public required TenantStatus TenantStatus { get; set; }

        [Required]
        [Column("TenantAddressId")]
        public required Guid TenantAddressId { get; set; }

        [Column("BillingAddressId")]
        public Guid? BillingAddressId { get; set; }

        [Required]
        [MaxLength(128)]
        [Column("Slug")]
        public required string Slug { get; set; }

        public Addresses TenantAddress { get; set; } = null!;
        public Addresses? BillingAddress { get; set; }
        public ICollection<Memberships> Memberships { get; set; } = new List<Memberships>();
        public ICollection<Subscriptions> Subscriptions { get; set; } = new List<Subscriptions>();
        public ICollection<Payments> Payments { get; set; } = new List<Payments>();
        public ICollection<MemberInvitations> MemberInvitations { get; set; } = new List<MemberInvitations>();
    }
}
