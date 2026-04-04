using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace SubscriptionPlatformApp.Application.Abstractions.Repositories.Shared
{
    public interface IRepositoryBase<T> where T : class
    {
        Task AddAsync(T entity, CancellationToken ct = default);
        void Update(T entity);
        void Remove(T entity);
    }
}
