using System;
using System.Collections.Generic;
using System.Text;

namespace SubscriptionPlatformApp.Application.DTOs.UseCases.AcceptMemberInvitationUseCase
{
    public class AcceptMemberInvitationUseCaseRequest
    {
        public required string Token { get; set; }
        public required bool IsNewUser { get; set; }
        public string? Fullname { get; set; }
        public string? Password { get; set; }
        public string? ConfirmPassword { get; set; }
    }
}
