using System;
using SubscriptionPlatformApp.Application.Abstractions.Repositories;

namespace SubscriptionPlatformApp.Application.Abstractions.Persistence;

public interface IUnitOfWork
{
    IUserRepository User { get; }
    IAddressRepository Address { get; }

    Task<int> SaveChangesAsync(CancellationToken ct = default);

    Task BeginTransactionAsync(CancellationToken ct = default);
    Task CommitTransactionAsync(CancellationToken ct = default);
    Task RollbackTransactionAsync(CancellationToken ct = default);
}
