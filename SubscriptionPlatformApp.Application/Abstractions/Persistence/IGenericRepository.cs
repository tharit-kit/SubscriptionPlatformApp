using System;

namespace SubscriptionPlatformApp.Application.Abstractions.Persistence;

public interface IGenericRepository<T> where T : class
{
    Task AddAsync(T entity, CancellationToken ct = default);
    void Update(T entity);
    void Remove(T entity);
}
