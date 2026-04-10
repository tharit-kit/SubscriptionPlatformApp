using System;
using System.Collections.Generic;
using System.Text;

namespace SubscriptionPlatformApp.Application.Helpers.AppSettings
{
    public sealed class SmtpSetting
    {
        public const string SectionName = "SmtpSetting";

        public string SenderName { get; init; } = default!;
        public string Email { get; init; } = default!;
        public string BaseUrl { get; init; } = "https://api.brevo.com/";
    }
}
