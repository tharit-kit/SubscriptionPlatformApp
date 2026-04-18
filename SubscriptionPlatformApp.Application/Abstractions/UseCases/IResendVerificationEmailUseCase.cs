using SubscriptionPlatformApp.Application.DTOs.UseCases;
using SubscriptionPlatformApp.Application.DTOs.UseCases.ResendVerificationEmailUseCase;
using System;
using System.Collections.Generic;
using System.Text;

namespace SubscriptionPlatformApp.Application.Abstractions.UseCases
{
    public interface IResendVerificationEmailUseCase
    {
        Task<ApiResponse<ResendVerificationEmailResponse>> ExecuteAsync(ResendVerificationEmailRequest request, CancellationToken ct);
    }
}
