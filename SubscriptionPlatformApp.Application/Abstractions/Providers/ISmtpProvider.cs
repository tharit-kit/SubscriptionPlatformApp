using SubscriptionPlatformApp.Application.DTOs.Providers.SmtpProvider;
using System;
using System.Collections.Generic;
using System.Text;

namespace SubscriptionPlatformApp.Application.Abstractions.Providers
{
    public interface ISmtpProvider
    {
        Task<bool> SendingEmailAsync(SendingEmailRequest message);
    }
}
