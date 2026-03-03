using System;
using SubscriptionPlatformApp.Application.Abstractions.Persistence;
using SubscriptionPlatformApp.Application.Abstractions.Repositories;
using SubscriptionPlatformApp.Domain.Entities;
using SubscriptionPlatformApp.Infrastructure.Persistence;
using SubscriptionPlatformApp.Infrastructure.Repositories.Shared;

namespace SubscriptionPlatformApp.Infrastructure.Repositories;

public class UserRepository : GenericRepository<Users>, IUserRepository 
{
    public UserRepository(DataContext db) : base(db) { }

    
}
