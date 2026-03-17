using SubscriptionPlatformApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SubscriptionPlatformApp.Application.Abstractions.Repositories
{
    public interface ITenantRepository
    {
        Task<Tenants?> FindBySlugAsync(string slug, CancellationToken ct);
        Task<bool> SlugExistsAsync(string slug, CancellationToken ct);
    }
}
