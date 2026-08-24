using System;
using System.Collections.Generic;
using System.Text;

namespace SubscriptionPlatformApp.Application.Abstractions.Services
{
    public interface ICurrentUserService
    {
        Guid? UserId { get; }
        string? Email { get; }
        bool IsAuthenticated { get; }
    }
}
