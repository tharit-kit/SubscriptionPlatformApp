using Microsoft.EntityFrameworkCore;
using SubscriptionPlatformApp.Application.Abstractions.Persistence;
using SubscriptionPlatformApp.Application.Abstractions.Repositories;
using SubscriptionPlatformApp.Domain.Entities;
using SubscriptionPlatformApp.Infrastructure.Persistence;
using SubscriptionPlatformApp.Infrastructure.Repositories.Shared;
using System;

namespace SubscriptionPlatformApp.Infrastructure.Repositories;

public class UserRepository : GenericRepository<Users>, IUserRepository 
{
    public UserRepository(AppDbContext db) : base(db) { }

    public Task<Users?> FindById(Guid id, CancellationToken ct)
    {
        return _set.FirstOrDefaultAsync(x => x.UserId == id, ct);
    }
}
