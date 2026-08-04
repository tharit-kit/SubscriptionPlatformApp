using System;
using System.Collections.Generic;
using System.Text;

namespace SubscriptionPlatformApp.Application.DTOs.UseCases.AcceptMemberInvitationUseCase
{
    public class AcceptMemberInvitationUseCaseResponse
    {
        public required string FullName { get; set; }
        public required string TenantName { get; set; }
    }
}
