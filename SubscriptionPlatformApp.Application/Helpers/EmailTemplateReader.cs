using System;
using System.Collections.Generic;
using System.Text;

namespace SubscriptionPlatformApp.Application.Helpers
{
    public static class EmailTemplateReader
    {
        public static string GetEmailTemplate(string templateFileName)
        {
            var assembly = typeof(EmailTemplateReader).Assembly;
            var resourceName = $"SubscriptionPlatformApp.Infrastructure.EmailTemplates.{templateFileName}";

            using var stream = assembly.GetManifestResourceStream(resourceName)
                ?? throw new FileNotFoundException($"Email template '{templateFileName}' not found as embedded resource");
            using var reader = new StreamReader(stream);
            return reader.ReadToEnd();
        }
    }
}
