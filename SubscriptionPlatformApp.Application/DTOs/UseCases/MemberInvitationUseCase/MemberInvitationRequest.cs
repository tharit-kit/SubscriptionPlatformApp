using System;
using System.Collections.Generic;
using System.Text;

namespace SubscriptionPlatformApp.Application.DTOs.UseCases.MemberInvitationUseCase
{
    public class MemberInvitationRequest
    {
        public required string Email { get; set; }
        public required string Role { get; set; }
    }
}
