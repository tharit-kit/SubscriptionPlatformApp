using SubscriptionPlatformApp.Application.Abstractions.Repositories;
using SubscriptionPlatformApp.Domain.Entities;
using SubscriptionPlatformApp.Infrastructure.Persistence;
using SubscriptionPlatformApp.Infrastructure.Repositories.Shared;
using System;
using System.Collections.Generic;
using System.Text;

namespace SubscriptionPlatformApp.Infrastructure.Repositories
{
    public class SubscriptionRepository : GenericRepository<Subscriptions>, ISubscriptionRepository
    {
        public SubscriptionRepository(AppDbContext db) : base(db)
        {
        }
    }
}
