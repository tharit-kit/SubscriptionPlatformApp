using System;
using System.Collections.Generic;
using System.Text;

namespace SubscriptionPlatformApp.Application.DTOs.UseCases.AcceptMemberInvitationUseCase
{
    public class AcceptMemberInvitationResponse
    {
        public required string FullName { get; set; }
        public required string TenantName { get; set; }
    }
}
