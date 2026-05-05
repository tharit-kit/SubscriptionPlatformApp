using System;
using System.Collections.Generic;
using System.Text;

namespace SubscriptionPlatformApp.Application.DTOs.Common
{
    public sealed class TenantContext
    {
        public Guid TenantId { get; init; }
        public string Slug { get; init; } = string.Empty;
        public Guid UserId { get; set; }
        public string Role { get; set; } = string.Empty;
    }
}
