using SubscriptionPlatformApp.Domain.Entities.Shared;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SubscriptionPlatformApp.Domain.Entities
{
    [Table("Users")]
    public class Users : BaseEntity
    {
        [Key]
        [Column("UserId")]
        public Guid UserId { get; set; }

        [Required]
        [Column("Email")]
        public required string Email { get; set; }

        [Column("HashedPassword")]
        public string? HashedPassword { get; set; }

        [Column("GeneratedSalt")]
        public string? GeneratedSalt { get; set; }

        [Column("TenantId")]
        public Guid TenantId { get; set; }

        [Column("UserStatus")]
        public string? UserStatus { get; set; }

        [Column("LastLoginAt")]
        public DateTime? LastLoginAt { get; set; }

        public ICollection<Memberships> Memberships { get; set; } = new List<Memberships>();
        public ICollection<MemberInvitation> InvitationsSent { get; set; } = new List<MemberInvitation>();
        public ICollection<EmailVerificationTokens> EmailVerificationTokens { get; set; } = new List<EmailVerificationTokens>();
    }
}
