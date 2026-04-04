using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SubscriptionPlatformApp.Application.Abstractions.Providers;
using SubscriptionPlatformApp.Application.DTOs.Providers.SmtpProvider;
using SubscriptionPlatformApp.Application.Helpers;
using SubscriptionPlatformApp.Application.Helpers.AppSettings;

namespace SubscriptionPlatformApp.Infrastructure.Providers
{
    public class EmailBaseProvider : IEmailBaseProvider
    {
        private readonly ISmtpProvider _smtpProvider;
        private readonly SmtpSetting _smtpSetting;
        private readonly ILogger<EmailBaseProvider> _logger;

        public EmailBaseProvider(ISmtpProvider smtpProvider, IOptions<SmtpSetting> smtpSetting, ILogger<EmailBaseProvider> logger)
        {
            _logger = logger;
            _smtpProvider = smtpProvider;
            _smtpSetting = smtpSetting.Value;
        }

        public async Task<bool> SendEmailAsync(
        string email,
        string fullName,
        string subject,
        string htmlContent,
        string emailType,
        CancellationToken ct = default)
        {
            var emailContent = new SendingEmailRequest
            {
                Subject = subject,
                Sender = new SenderInfo
                {
                    Email = _smtpSetting.Email,
                    Name = _smtpSetting.SenderName
                },
                HtmlContent = EmailTemplateReader.GetEmailTemplate("UserVerificationEmailTemplate.html"),
                To =
                [
                    new RecepientInfo
                    {
                        Email = email,
                        Name = fullName
                    }
                ]
            };

            var result = await _smtpProvider.SendingEmailAsync(emailContent);

            if (!result)
            {
                _logger.LogWarning("Failed to send {EmailType} email to {Email}", emailType, email);
            }
            else
            {
                _logger.LogInformation("{EmailType} email sent successfully to {Email}", emailType, email);
            }

            return result;
        }
    }
}
