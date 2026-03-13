using System;
using SubscriptionPlatformApp.Application.Abstractions.Persistence;
using SubscriptionPlatformApp.Application.Abstractions.Repositories;
using SubscriptionPlatformApp.Domain.Entities;
using SubscriptionPlatformApp.Infrastructure.Repositories.Shared;

namespace SubscriptionPlatformApp.Infrastructure.Repositories;

public class UserRepository : GenericRepository<Users>, IUserRepository 
{
    public UserRepository(Persistence.AppDbContext db) : base(db) { }

    
}
