using System;
using System.Collections.Generic;
using System.Text;

namespace SubscriptionPlatformApp.Application.Helpers.AppSettings
{
    public sealed class FrontendSetting
    {
        public const string SectionName = "FrontendSetting";
        public string BaseUrl { get; init; } = default!;
    }
}
