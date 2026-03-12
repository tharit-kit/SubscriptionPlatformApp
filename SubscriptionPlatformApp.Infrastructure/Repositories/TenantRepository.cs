using SubscriptionPlatformApp.Application.Abstractions.Repositories;
using SubscriptionPlatformApp.Domain.Entities;
using SubscriptionPlatformApp.Infrastructure.AppDbContext;
using SubscriptionPlatformApp.Infrastructure.Repositories.Shared;
using System;
using System.Collections.Generic;
using System.Text;

namespace SubscriptionPlatformApp.Infrastructure.Repositories
{
    public class TenantRepository : GenericRepository<Tenants>, ITenantRepository
    {
        public TenantRepository(DataContext context) : base(context)
        {
        }
    }
}
