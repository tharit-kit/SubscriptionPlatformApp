using System;
using SubscriptionPlatformApp.Application.Abstractions.Repositories;

namespace SubscriptionPlatformApp.Application.Abstractions.Persistence;

public interface IUnitOfWork : IDisposable
{
    IUserRepository User { get; }
    IAddressRepository Address { get; }
    IEmailVerificationTokenRepository EmailVerificationToken { get; }
    IMemberInvitationRepository MemberInvitation { get; }
    IMembershipRepository MemberMembership { get; }
    ISubscriptionRepository Subscription { get; }
    IPaymentRepository Payment { get; }
    ITenantRepository Tenant { get; }

    Task<int> SaveChangesAsync(CancellationToken ct = default);

    Task BeginTransactionAsync(CancellationToken ct = default);
    Task CommitTransactionAsync(CancellationToken ct = default);
    Task RollbackTransactionAsync(CancellationToken ct = default);
}
