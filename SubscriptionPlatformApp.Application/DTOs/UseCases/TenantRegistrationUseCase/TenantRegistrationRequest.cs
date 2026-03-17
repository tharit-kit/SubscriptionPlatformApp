using System;
using System.Collections.Generic;
using System.Text;

namespace SubscriptionPlatformApp.Application.DTOs.UseCases.TenantRegistrationUseCase
{
    public class TenantRegistrationRequest
    {
        public required TenantInfomation TenantInfo { get; set; }
        public required TenantAddress TenantAddress { get; set; }
        public required NewAdmin NewAdmin { get; set; }
    }

    public class TenantInfomation
    {
        public required string TenantName { get; set; }
        public required string BusinessType { get; set; }
        public required string Subdomain { get; set; }
    }

    public class TenantAddress
    {
        public required string Address1 { get; set; }
        public string? Address2 { get; set; }
        public required string Country { get; set; }
        public required string District { get; set; }
        public required string SubDistrict { get; set; }
        public required string Province { get; set; }
        public required string Zipcode { get; set; }
    }

    public class NewAdmin
    {
        public required string DisplayName { get; set; }
        public required string Email { get; set; }
        public required string Password { get; set; }
        public required string ConfirmPassword { get; set; }
    }
}
