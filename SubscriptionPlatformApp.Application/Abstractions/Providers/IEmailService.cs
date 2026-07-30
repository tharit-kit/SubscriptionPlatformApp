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

        Task<bool> SendMemberInvitationEmailAsync(
            string email,
            string fullName,
            string tenantName,
            string inviterName,
            string roleName,
            string memberInvitationToken,
            DateTime expirationDate,
            CancellationToken ct);
    }
}
