using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SubscriptionPlatformApp.Domain.Entities
{
    [Table("Memberships")]
    public class Memberships
    {
        [Key]
        [Column("MembershipId")]
        public Guid MembershipId { get; set; }

        [Column("BusinessId")]
        public Guid BusinessId { get; set; }

        [Column("UserId")]
        public Guid UserId { get; set; }

        [Column("Role")]
        public string? Role { get; set; }

        [Column("MemberStatus")]
        public string? MemberStatus { get; set; }

        [Column("JoinedAt")]
        public DateTime? JoinedAt { get; set; }

        public Tenants Tenant { get; set; } = null!;

        public Users User { get; set; } = null!;
    }
}
