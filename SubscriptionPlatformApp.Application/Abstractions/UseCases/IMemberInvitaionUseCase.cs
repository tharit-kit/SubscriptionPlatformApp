using SubscriptionPlatformApp.Application.DTOs.UseCases;
using SubscriptionPlatformApp.Application.DTOs.UseCases.MemberInvitationUseCase;
using System;
using System.Collections.Generic;
using System.Text;

namespace SubscriptionPlatformApp.Application.Abstractions.UseCases
{
    public interface IMemberInvitaionUseCase
    {
        Task<ApiResponse<MemberInvitationResponse>> ExecuteAsync(MemberInvitationRequest request, CancellationToken ct);
    }
}
