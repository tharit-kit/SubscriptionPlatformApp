using System;
using System.Collections.Generic;
using System.Text;

namespace SubscriptionPlatformApp.Application.DTOs.UseCases.LoginUseCase
{
    public class LoginResponse
    {
        public required string AccessToken { get; set; }
        public List<MembershipInfo>? Memberships { get; set; }
    }

    public class MembershipInfo { 
        public required string TenantName { get; set; }
        public required string Slug { get; set; }
        public required string Role { get; set; }
    }
}
