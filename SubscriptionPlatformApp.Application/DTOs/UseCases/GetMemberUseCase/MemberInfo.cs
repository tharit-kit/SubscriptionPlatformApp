using System;
using System.Collections.Generic;
using System.Text;

namespace SubscriptionPlatformApp.Application.DTOs.UseCases.GetMemberUseCase
{
    public class MemberInfo
    {
        public required string FullName { get; set; }
        public required string Role { get; set; }
        public required string MemberStatus { get; set; }
        public string? JoinAt { get; set; }
    }
}
