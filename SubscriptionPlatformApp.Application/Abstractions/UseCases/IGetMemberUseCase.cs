using SubscriptionPlatformApp.Application.DTOs.UseCases;
using SubscriptionPlatformApp.Application.DTOs.UseCases.GetMemberUseCase;
using System;
using System.Collections.Generic;
using System.Text;

namespace SubscriptionPlatformApp.Application.Abstractions.UseCases
{
    public interface IGetMemberUseCase
    {
        Task<ApiResponse<GetMemberResponse>> ExecuteAsync(CancellationToken ct);
    }
}
