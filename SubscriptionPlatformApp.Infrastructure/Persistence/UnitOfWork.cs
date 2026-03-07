using System;
using Microsoft.EntityFrameworkCore.Storage;
using SubscriptionPlatformApp.Application.Abstractions.Persistence;
using SubscriptionPlatformApp.Application.Abstractions.Repositories;
using SubscriptionPlatformApp.Domain.Entities;
using SubscriptionPlatformApp.Infrastructure.Repositories;

namespace SubscriptionPlatformApp.Infrastructure.Persistence;

public class UnitOfWork : IUnitOfWork
{
    private readonly DataContext _db;
    private IDbContextTransaction? _tx;

    public UnitOfWork(DataContext db)
    {
        _db = db;
        User = new UserRepository(_db);
        Address = new AddressRepository(_db);
        // Businesses = new EfRepository<Business>(_db);
    }

    public IUserRepository User { get; }
    public IAddressRepository Address { get; }
    // public IRepository<Business> Businesses { get; }

    public Task<int> SaveChangesAsync(CancellationToken ct = default)
        => _db.SaveChangesAsync(ct);

    public async Task BeginTransactionAsync(CancellationToken ct = default)
        => _tx = await _db.Database.BeginTransactionAsync(ct);

    public async Task CommitTransactionAsync(CancellationToken ct = default)
    {
        await _db.SaveChangesAsync(ct);
        if (_tx != null) await _tx.CommitAsync(ct);
    }

    public async Task RollbackTransactionAsync(CancellationToken ct = default)
    {
        if (_tx != null) await _tx.RollbackAsync(ct);
    }
}
