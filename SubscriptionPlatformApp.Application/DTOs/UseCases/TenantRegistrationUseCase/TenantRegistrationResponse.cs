using System;
using System.Collections.Generic;
using System.Text;

namespace SubscriptionPlatformApp.Application.DTOs.UseCases.TenantRegistrationUseCase
{
    public class TenantRegistrationResponse
    {
        public Guid TenantId { get; set; }
        public Guid AdminId { get; set; }
    }
}
