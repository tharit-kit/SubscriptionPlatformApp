using System;
using System.Collections.Generic;
using System.Text;

namespace SubscriptionPlatformApp.Application.DTOs.UseCases.LoginUseCase
{
    public class LoginRequest
    {
        public required string Email { get; set; }
        public required string Password { get; set; }
    }
}
