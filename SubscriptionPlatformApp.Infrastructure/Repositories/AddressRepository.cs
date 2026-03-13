using System;
using SubscriptionPlatformApp.Application.Abstractions.Repositories;
using SubscriptionPlatformApp.Domain.Entities;
using SubscriptionPlatformApp.Infrastructure.Repositories.Shared;

namespace SubscriptionPlatformApp.Infrastructure.Repositories;

public class AddressRepository : GenericRepository<Addresses>, IAddressRepository 
{
    public AddressRepository(Persistence.AppDbContext db) : base(db) { }
}
