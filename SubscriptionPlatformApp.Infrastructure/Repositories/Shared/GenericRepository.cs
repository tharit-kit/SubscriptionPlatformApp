using System;
using Microsoft.EntityFrameworkCore;
using SubscriptionPlatformApp.Application.Abstractions.Persistence;
using SubscriptionPlatformApp.Infrastructure.Persistence;

namespace SubscriptionPlatformApp.Infrastructure.Repositories.Shared;

public class GenericRepository<T> : IGenericRepository<T> where T : class
{
    private readonly DataContext _context;
    private readonly DbSet<T> _set;
    public GenericRepository(
        DataContext context
    )
    {
        _context = context;
        _set = _context.Set<T>();
    }

    public Task AddAsync(T entity, CancellationToken ct = default)
        => _set.AddAsync(entity, ct).AsTask();

    public void Update(T entity) => _set.Update(entity);
    
    public void Remove(T entity) => _set.Remove(entity);
}
