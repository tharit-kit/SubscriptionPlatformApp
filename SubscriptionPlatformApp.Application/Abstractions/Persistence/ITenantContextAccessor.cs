using SubscriptionPlatformApp.Application.DTOs.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace SubscriptionPlatformApp.Application.Abstractions.Persistence
{
    public interface ITenantContextAccessor
    {
        TenantContext? Current { get; set; }
    }
}
