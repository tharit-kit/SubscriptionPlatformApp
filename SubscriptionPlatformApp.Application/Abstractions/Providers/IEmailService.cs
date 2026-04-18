using System;
using System.Collections.Generic;
using System.Text;

namespace SubscriptionPlatformApp.Application.Abstractions.Providers
{
    public interface IEmailService
    {
        Task<bool> SendVerificationEmailAsync(
            string email,
            string fullName,
            Guid verificationToken,
            CancellationToken ct);
    }
}
