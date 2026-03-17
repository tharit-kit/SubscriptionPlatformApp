using Microsoft.EntityFrameworkCore;
using SubscriptionPlatformApp.Application.Abstractions.Repositories;
using SubscriptionPlatformApp.Domain.Entities;
using SubscriptionPlatformApp.Infrastructure.Persistence;
using SubscriptionPlatformApp.Infrastructure.Repositories.Shared;
using System;
using System.Collections.Generic;
using System.Text;

namespace SubscriptionPlatformApp.Infrastructure.Repositories
{
    public class TenantRepository : GenericRepository<Tenants>, ITenantRepository
    {
        public TenantRepository(AppDbContext db) : base(db)
        {
        }

        public Task<Tenants?> FindBySlugAsync(string slug, CancellationToken ct)
        {
            return _set.FirstOrDefaultAsync(x => x.Slug == slug, ct);
        }

        public Task<bool> SlugExistsAsync(string slug, CancellationToken ct)
        {
            return _set.AnyAsync(x => x.Slug == slug, ct);
        }
    }
}
