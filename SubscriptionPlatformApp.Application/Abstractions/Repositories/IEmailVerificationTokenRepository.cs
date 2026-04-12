using SubscriptionPlatformApp.Application.Abstractions.Repositories.Shared;
using SubscriptionPlatformApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SubscriptionPlatformApp.Application.Abstractions.Repositories
{
    public interface IEmailVerificationTokenRepository : IRepositoryBase<EmailVerificationTokens>
    {
        Task<EmailVerificationTokens?> FindByIdAsync(Guid id, CancellationToken ct);
    }
}
