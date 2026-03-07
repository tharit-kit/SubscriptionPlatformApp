using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using SubscriptionPlatformApp.Domain.Entities.Shared;

namespace SubscriptionPlatformApp.Domain.Entities
{
    [Table("Memberships")]
    public class Memberships : BaseEntity
    {
        [Key]
        [Column("MembershipId")]
        public Guid MembershipId { get; set; }

        [Required]
        [Column("TenantId")]
        public Guid TenantId { get; set; }

        [Required]
        [Column("UserId")]
        public Guid UserId { get; set; }

        [MaxLength(64)]
        [Column("Role")]
        public string? Role { get; set; }

        [MaxLength(64)]
        [Column("MemberStatus")]
        public string? MemberStatus { get; set; }

        [Column("JoinedAt")]
        public DateTime? JoinedAt { get; set; }

        public Tenants Tenant { get; set; } = null!;
        public Users User { get; set; } = null!;
    }
}
