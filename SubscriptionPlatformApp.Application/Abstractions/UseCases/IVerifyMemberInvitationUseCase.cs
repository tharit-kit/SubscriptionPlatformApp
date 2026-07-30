using SubscriptionPlatformApp.Application.DTOs.UseCases;
using SubscriptionPlatformApp.Application.DTOs.UseCases.VerifyMemberInvitation;
using System;
using System.Collections.Generic;
using System.Text;

namespace SubscriptionPlatformApp.Application.Abstractions.UseCases
{
    public interface IVerifyMemberInvitationUseCase
    {
        Task<ApiResponse<VerifyMemberInvitationResponse>> ExecuteAsync(VerifyMemberInvitationRequest request, CancellationToken ct);
    }
}
