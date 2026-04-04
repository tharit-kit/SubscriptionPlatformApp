using System;
using System.Collections.Generic;
using System.Text;

namespace SubscriptionPlatformApp.Application.Abstractions.Providers
{
    public interface IEmailBaseProvider
    {
        Task<bool> SendEmailAsync(
            string email,
            string fullName,
            string subject,
            string htmlContent,
            string emailType,
            CancellationToken ct = default);
    }
}
