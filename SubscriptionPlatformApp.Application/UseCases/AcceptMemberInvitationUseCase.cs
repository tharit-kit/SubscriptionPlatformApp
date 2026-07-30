using Microsoft.Extensions.Logging;
using SubscriptionPlatformApp.Application.Abstractions.Persistence;
using SubscriptionPlatformApp.Application.DTOs.UseCases;
using SubscriptionPlatformApp.Application.DTOs.UseCases.VerifyMemberInvitation;
using SubscriptionPlatformApp.Application.Utils.Response;
using System;
using System.Collections.Generic;
using System.Text;

namespace SubscriptionPlatformApp.Application.UseCases
{
    public class AcceptMemberInvitationUseCase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<AcceptMemberInvitationUseCase> _logger;

        public AcceptMemberInvitationUseCase(IUnitOfWork unitOfWork, ILogger<AcceptMemberInvitationUseCase> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<ApiResponse<AcceptMemberInvitationUseCaseResponse>> ExecuteAsync(AcceptMemberInvitationUseCaseRequest request, CancellationToken ct)
        {
            try
            {
                var invitation = await _unitOfWork.MemberInvitation.FindByToken(request.Token, ct);
                if (invitation == null)
                {
                    return ApiResponse.Fail<AcceptMemberInvitationUseCaseResponse>(ResponseCodes.MemberInvitationNotFound);
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

                return ApiResponse.Success<AcceptMemberInvitationUseCaseResponse>(res);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Something went wrong");
                return ApiResponse.Fail<AcceptMemberInvitationUseCaseResponse>(ResponseCodes.SystemError);
            }
        }
    }
}
