using SubscriptionPlatformApp.Application.Abstractions.Repositories.Shared;
using SubscriptionPlatformApp.Domain.Entities;
using System;

namespace SubscriptionPlatformApp.Application.Abstractions.Repositories;

public interface IUserRepository : IRepositoryBase<Users>
{
    Task<Users?> FindById(Guid id, CancellationToken ct);
}
