using SubscriptionPlatformApp.Application.Abstractions.Repositories;
using SubscriptionPlatformApp.Domain.Entities;
using SubscriptionPlatformApp.Infrastructure.AppDbContext;
using SubscriptionPlatformApp.Infrastructure.Repositories.Shared;
using System;
using System.Collections.Generic;
using System.Text;

namespace SubscriptionPlatformApp.Infrastructure.Repositories
{
    public class PaymentRepository: GenericRepository<Payments>, IPaymentRepository
    {
        public PaymentRepository(DataContext db) : base(db) { }
    }
}
