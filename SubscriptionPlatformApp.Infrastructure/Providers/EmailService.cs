using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SubscriptionPlatformApp.Application.Abstractions.Providers;
using SubscriptionPlatformApp.Application.DTOs.Providers.SmtpProvider;
using SubscriptionPlatformApp.Application.Helpers.AppSettings;

namespace SubscriptionPlatformApp.Infrastructure.Providers
{
    public class EmailService : IEmailService
    {
        private readonly ISmtpProvider _smtpProvider;
        private readonly SmtpSetting _smtpSetting;
        private readonly FrontendSetting _frontendSetting;
        private readonly ILogger<EmailService> _logger;

        public EmailService(ISmtpProvider smtpProvider, IOptions<SmtpSetting> smtpSetting, IOptions<FrontendSetting> frontendSetting, ILogger<EmailService> logger)
        {
            _logger = logger;
            _smtpProvider = smtpProvider;
            _smtpSetting = smtpSetting.Value;
            _frontendSetting = frontendSetting.Value;
        }

        public async Task<bool> SendVerificationEmailAsync(
            string email,
            string fullName,
            Guid verificationToken,
            CancellationToken ct)
        {
            var template = GetEmailTemplate("EmailVerificationEmailTemplate.html");
            var verificationLink = $"{_frontendSetting.BaseUrl}/verify-email?token={verificationToken}";

            var htmlContent = template
                .Replace("{{VERIFICATION_LINK}}", verificationLink);

            return await SendEmailAsync(
                email,
                fullName,
                "Email Verification",
                htmlContent,
                "Verification",
                ct
            );
        }

        private async Task<bool> SendEmailAsync(
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
                HtmlContent = htmlContent,
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

        private string GetEmailTemplate(string templateFileName)
        {
            var assembly = typeof(EmailService).Assembly;
            var resourceName = $"SubscriptionPlatformApp.Infrastructure.EmailTemplates.{templateFileName}";

            using var stream = assembly.GetManifestResourceStream(resourceName)
                ?? throw new FileNotFoundException($"Email template '{templateFileName}' not found as embedded resource");
            using var reader = new StreamReader(stream);
            return reader.ReadToEnd();
        }
    }
}
