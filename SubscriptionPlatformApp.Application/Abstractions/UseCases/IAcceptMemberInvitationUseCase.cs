using SubscriptionPlatformApp.Application.DTOs.UseCases;
using SubscriptionPlatformApp.Application.DTOs.UseCases.AcceptMemberInvitationUseCase;
using System;
using System.Collections.Generic;
using System.Text;

namespace SubscriptionPlatformApp.Application.Abstractions.UseCases
{
    public interface IAcceptMemberInvitationUseCase
    {
        Task<ApiResponse<AcceptMemberInvitationUseCaseResponse>> ExecuteAsync(AcceptMemberInvitationUseCaseRequest request, CancellationToken ct);
    }
}
