using System;
using System.Collections.Generic;
using System.Text;

namespace SubscriptionPlatformApp.Application.Abstractions.Services
{
    public interface ICurrentUserService
    {
        Guid UserId { get; }
        Guid TenantId { get; }
        string? Role { get; }
        bool IsAuthenticated { get; }
    }
}
