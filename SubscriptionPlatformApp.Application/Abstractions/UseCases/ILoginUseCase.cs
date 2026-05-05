using SubscriptionPlatformApp.Application.DTOs.UseCases;
using SubscriptionPlatformApp.Application.DTOs.UseCases.LoginUseCase;
using System;
using System.Collections.Generic;
using System.Text;

namespace SubscriptionPlatformApp.Application.Abstractions.UseCases
{
    public interface ILoginUseCase
    {
        Task<ApiResponse<LoginResponse>> ExecuteAsync(LoginRequest request, CancellationToken ct);
    }
}
