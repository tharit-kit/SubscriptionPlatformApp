using SubscriptionPlatformApp.Domain.Entities.Shared;
using SubscriptionPlatformApp.Domain.Enums;
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
        [MaxLength(128)]
        [Column("Email")]
        public required string Email { get; set; }

        [MaxLength(128)]
        [Column("HashedPassword")]
        public string? HashedPassword { get; set; }

        [MaxLength(128)]
        [Column("GeneratedSalt")]
        public string? GeneratedSalt { get; set; }

        [Required]
        [Column("UserStatus")]
        public required UserStatus UserStatus { get; set; }

        [Column("LastLoginAt")]
        public DateTime? LastLoginAt { get; set; }

        public ICollection<Memberships> Memberships { get; set; } = new List<Memberships>();
        public ICollection<MemberInvitations> InvitationsSent { get; set; } = new List<MemberInvitations>();
        public ICollection<MemberInvitations> InvitationsReceived { get; set; } = new List<MemberInvitations>();
        public ICollection<EmailVerificationTokens> EmailVerificationTokens { get; set; } = new List<EmailVerificationTokens>();
    }
}
