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
    public class MembershipRepository : GenericRepository<Memberships>, IMembershipRepository
    {
        public MembershipRepository(AppDbContext db) : base(db) { }

        public Task<Memberships?> FindByTenantIdAndUserIdAsync(Guid tenantId, Guid userId, CancellationToken ct)
        {
            return _set.FirstOrDefaultAsync(x => x.UserId == userId && x.TenantId == tenantId, ct);
        }
    }
}
