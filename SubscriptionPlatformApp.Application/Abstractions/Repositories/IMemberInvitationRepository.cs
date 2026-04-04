using SubscriptionPlatformApp.Application.Abstractions.Repositories.Shared;
using SubscriptionPlatformApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SubscriptionPlatformApp.Application.Abstractions.Repositories
{
    public interface IMemberInvitationRepository : IRepositoryBase<MemberInvitations>
    {
    }
}
