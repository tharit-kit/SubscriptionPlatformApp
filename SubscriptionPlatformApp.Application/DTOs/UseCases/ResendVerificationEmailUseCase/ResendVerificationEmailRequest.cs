using System;
using System.Collections.Generic;
using System.Text;

namespace SubscriptionPlatformApp.Application.DTOs.UseCases.ResendVerificationEmailUseCase
{
    public class ResendVerificationEmailRequest
    {
        public Guid UserId { get; set; }
        public Guid TenantId { get; set; }
    }
}
