using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using SubscriptionPlatformApp.Domain.Entities.Shared;

namespace SubscriptionPlatformApp.Domain.Entities;

[Table("MemberInvitations")]
public class MemberInvitations : BaseEntity
{
    [Key]
    [Column("MemberInvitaionId")]
    public Guid MemberInvitationId { get; set; }

    [Column("TenantId")]
    public Guid TenantId { get; set; }
    
    [Required]
    [MaxLength(128)]
    [Column("InvitedEmail")]
    public required string InvitedEmail { get; set; }

    [Column("InvitedUserId")]
    public Guid? InvitedUserId { get; set; }
    
    [Column("InvitedByUserId")]
    public Guid InvitedByUserId { get; set; }
    
    [Required]
    [MaxLength(64)]
    [Column("Role")]
    public required string Role { get; set; }

    [Required]
    [MaxLength(64)]
    [Column("InvitationStatus")]
    public required string InvitationStatus { get; set; }

    [Required]
    [MaxLength(128)]
    [Column("HashedToken")]
    public required string HashedToken { get; set; }

    [Column("ExpiresAt")]
    public DateTime ExpiresAt { get; set; }

    [Column("AcceptedAt")]
    public DateTime? AcceptedAt { get; set; }
    
    public Tenants Tenant { get; set; } = null!;
    public Users? InvitedUser { get; set; }
    public Users InvitedByUser { get; set; } = null!;
}
