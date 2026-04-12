using SubscriptionPlatformApp.Application.DTOs.UseCases;
using SubscriptionPlatformApp.Application.DTOs.UseCases.UserVerificationUseCase;
using System;
using System.Collections.Generic;
using System.Text;

namespace SubscriptionPlatformApp.Application.Abstractions.UseCases
{
    public interface IEmailVerificationUseCase
    {
        Task<ApiResponse<EmailVerificationResponse>> ExecuteAsync(Guid tokenId, CancellationToken ct);
    }
}
