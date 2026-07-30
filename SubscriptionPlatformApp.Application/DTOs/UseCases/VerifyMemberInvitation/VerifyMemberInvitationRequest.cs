using System;
using System.Collections.Generic;
using System.Text;

namespace SubscriptionPlatformApp.Application.DTOs.UseCases.VerifyMemberInvitation
{
    public class VerifyMemberInvitationRequest
    {
        public required string Token { get; set; }
    }
}
