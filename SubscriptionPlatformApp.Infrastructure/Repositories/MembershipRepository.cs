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
            return _set.IgnoreQueryFilters().FirstOrDefaultAsync(x => x.UserId == userId && x.TenantId == tenantId, ct);
        }

        public Task<List<Memberships>> FindByUserId(Guid userId, CancellationToken ct)
        {
            return _set.IgnoreQueryFilters()
                .Where(x => x.UserId == userId)
                .Include(x => x.Tenant)
                .ToListAsync(ct);
        }

        public Task<List<Memberships>> GetMembershipByTenantId(CancellationToken ct)
        {
            return _set.Include(x => x.User).ToListAsync(ct);
        }
    }
}
