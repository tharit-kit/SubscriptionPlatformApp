using SubscriptionPlatformApp.Application.DTOs.UseCases;
using SubscriptionPlatformApp.Application.DTOs.UseCases.TenantRegistrationUseCase;
using System;
using System.Collections.Generic;
using System.Text;

namespace SubscriptionPlatformApp.Application.Abstractions.UseCases
{
    public interface ITenantRegistrationUseCase
    {
        Task<ApiResponse<TenantRegistrationResponse>> ExecuteAsync(TenantRegistrationRequest request, CancellationToken ct);
    }
}
