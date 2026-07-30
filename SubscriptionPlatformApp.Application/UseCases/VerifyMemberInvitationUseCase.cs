using Microsoft.Extensions.Logging;
using SubscriptionPlatformApp.Application.Abstractions.Persistence;
using SubscriptionPlatformApp.Application.Abstractions.Providers;
using SubscriptionPlatformApp.Application.Abstractions.Services;
using SubscriptionPlatformApp.Application.Abstractions.UseCases;
using SubscriptionPlatformApp.Application.DTOs.UseCases;
using SubscriptionPlatformApp.Application.DTOs.UseCases.MemberInvitationUseCase;
using SubscriptionPlatformApp.Application.DTOs.UseCases.VerifyMemberInvitation;
using SubscriptionPlatformApp.Application.Utils.Constants;
using SubscriptionPlatformApp.Application.Utils.Response;
using SubscriptionPlatformApp.Domain.Entities;
using SubscriptionPlatformApp.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace SubscriptionPlatformApp.Application.UseCases
{
    public class VerifyMemberInvitationUseCase : IVerifyMemberInvitationUseCase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<VerifyMemberInvitationUseCase> _logger;

        public VerifyMemberInvitationUseCase(IUnitOfWork unitOfWork, ILogger<VerifyMemberInvitationUseCase> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<ApiResponse<VerifyMemberInvitationResponse>> ExecuteAsync(VerifyMemberInvitationRequest request, CancellationToken ct)
        {
            try
            {
                var invitation = await _unitOfWork.MemberInvitation.FindByToken(request.Token, ct);
                if (invitation == null)
                {
                    return ApiResponse.Fail<VerifyMemberInvitationResponse>(ResponseCodes.MemberInvitationNotFound);
                }

                var isNewUser = true;
                var invitee = await _unitOfWork.User.FindByEmail(invitation.InvitedEmail, ct);
                if (invitee != null) 
                {
                    isNewUser = false;
                }

                var res = new VerifyMemberInvitationResponse
                {
                    IsNewUser = isNewUser,
                };

                return ApiResponse.Success<VerifyMemberInvitationResponse>(res);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Something went wrong");
                return ApiResponse.Fail<VerifyMemberInvitationResponse>(ResponseCodes.SystemError);
            }
        }
    }
}
