using SubscriptionPlatformApp.Domain.Entities.Shared;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SubscriptionPlatformApp.Domain.Entities
{
    [Table("EmailVerificationTokens")]
    public class EmailVerificationTokens : BaseEntity
    {
        [Key]
        [Column("EmailVerificationTokenId")]
        public Guid EmailVerificationTokenId { get; set; }

        [Required]
        public Guid UserId { get; set; }
        
        [Required]
        public DateTime ExpireAt { get; set; }

        public Users User { get; set; } = null!;
    }
}
