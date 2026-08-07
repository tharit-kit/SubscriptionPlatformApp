using System;
using System.Collections.Generic;
using System.Text;

namespace SubscriptionPlatformApp.Application.DTOs.UseCases.VerifyMemberInvitation
{
    public class VerifyMemberInvitationResponse
    {
        public bool IsNewUser { get; set; }
        public required string Email { get; set; }
    }
}
