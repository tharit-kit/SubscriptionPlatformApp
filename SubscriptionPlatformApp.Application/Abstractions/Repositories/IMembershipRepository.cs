using SubscriptionPlatformApp.Application.Abstractions.Repositories.Shared;
using SubscriptionPlatformApp.Domain.Entities;

namespace SubscriptionPlatformApp.Application.Abstractions.Repositories
{
    public interface IMembershipRepository : IRepositoryBase<Memberships>
    {
        Task<Memberships?> FindByTenantIdAndUserIdAsync(Guid tenantId, Guid userId, CancellationToken ct);
    }
}
