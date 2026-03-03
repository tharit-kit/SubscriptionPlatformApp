using System;
using SubscriptionPlatformApp.Application.Abstractions.Persistence;
using SubscriptionPlatformApp.Application.Abstractions.Repositories;
using SubscriptionPlatformApp.Domain.Entities;
using SubscriptionPlatformApp.Infrastructure.Repositories;

namespace SubscriptionPlatformApp.Infrastructure.Persistence;

public class UnitOfWork : IUnitOfWork
{
    private readonly DataContext _db;

    public UnitOfWork(DataContext db)
    {
        _db = db;
        Users = new UserRepository(_db);
        // Businesses = new EfRepository<Business>(_db);
    }

    public IUserRepository Users { get; }
    // public IRepository<Business> Businesses { get; }

    public Task<int> SaveChangesAsync(CancellationToken ct = default)
        => _db.SaveChangesAsync(ct);
}
