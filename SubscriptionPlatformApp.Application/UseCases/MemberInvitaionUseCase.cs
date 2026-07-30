using Microsoft.Extensions.Logging;
using SubscriptionPlatformApp.Application.Abstractions.Persistence;
using SubscriptionPlatformApp.Application.Abstractions.Providers;
using SubscriptionPlatformApp.Application.Abstractions.Services;
using SubscriptionPlatformApp.Application.Abstractions.UseCases;
using SubscriptionPlatformApp.Application.DTOs.UseCases;
using SubscriptionPlatformApp.Application.DTOs.UseCases.GetMemberUseCase;
using SubscriptionPlatformApp.Application.DTOs.UseCases.MemberInvitationUseCase;
using SubscriptionPlatformApp.Application.Utils.Constants;
using SubscriptionPlatformApp.Application.Utils.Response;
using SubscriptionPlatformApp.Domain.Entities;
using SubscriptionPlatformApp.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace SubscriptionPlatformApp.Application.UseCases
{
    public class MemberInvitaionUseCase : IMemberInvitaionUseCase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;
        private readonly ISecureTokenGenerator _secureTokenGenerator;
        private readonly IEmailService _emailService;
        private readonly ILogger<MemberInvitaionUseCase> _logger;

        public MemberInvitaionUseCase(IUnitOfWork unitOfWork,
                                      ISecureTokenGenerator secureTokenGenerator,
                                      ICurrentUserService currentUserService,
                                      IEmailService emailService,
                                      ILogger<MemberInvitaionUseCase> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _currentUserService = currentUserService;
            _secureTokenGenerator = secureTokenGenerator;
            _emailService = emailService;
        }

        public async Task<ApiResponse<MemberInvitationResponse>> ExecuteAsync(MemberInvitationRequest request, CancellationToken ct)
        {
            try
            {
                if (!_currentUserService.IsAuthenticated)
                {
                    return ApiResponse.Fail<MemberInvitationResponse>(ResponseCodes.Unauthorized);
                }

                if (_currentUserService.Role != RoleConstants.ADMIN_ROLE)
                {
                    return ApiResponse.Fail<MemberInvitationResponse>(ResponseCodes.InsufficientPrivilege);
                }

                var inviterMembership = await _unitOfWork.Membership.FindByUserId(_currentUserService.UserId, true, ct);
                if (inviterMembership == null)
                {
                    return ApiResponse.Fail<MemberInvitationResponse>(ResponseCodes.MembershipNotFound);
                }

                var invitee = await _unitOfWork.User.FindByEmail(request.Email, ct);
                if (invitee != null)
                {
                    var inviteeMembership = await _unitOfWork.Membership.FindByUserId(invitee.UserId, true, ct);
                    if (inviteeMembership != null)
                    {
                        return ApiResponse.Fail<MemberInvitationResponse>(ResponseCodes.UserAlreadyInTenant);
                    }
                }

                var tenant = await _unitOfWork.Tenant.FindByIdAsync(_currentUserService.TenantId, ct);
                if (tenant == null)
                {
                    return ApiResponse.Fail<MemberInvitationResponse>(ResponseCodes.TenantNotFound);
                }

                var inviter = await _unitOfWork.User.FindById(_currentUserService.UserId, ct);
                if (inviter == null)
                {
                    return ApiResponse.Fail<MemberInvitationResponse>(ResponseCodes.UserNotFound);
                }

                var invitation = new MemberInvitations
                {
                    MemberInvitationId = Guid.NewGuid(),
                    TenantId = _currentUserService.TenantId,
                    InvitedEmail = request.Email,
                    Role = request.Role,
                    HashedToken = _secureTokenGenerator.Generate().Hash,
                    CreatedBy = _currentUserService.UserId,
                    CreatedAt = DateTime.UtcNow,
                    InvitationStatus = InvitationStatus.Invited,
                    ExpiresAt = DateTime.UtcNow.AddMinutes(15),
                };

                await _unitOfWork.MemberInvitation.AddAsync(invitation, ct);
                await _unitOfWork.SaveChangesAsync(ct);

                var isSendEmail = await _emailService.SendMemberInvitationEmailAsync(request.Email,
                                                                                     "New Member",
                                                                                     tenant.TenantName,
                                                                                     inviter.FullName,
                                                                                     request.Role,
                                                                                     invitation.HashedToken,
                                                                                     invitation.ExpiresAt,
                                                                                     ct);
                if (isSendEmail)
                {
                    _logger.LogInformation("Member invitation email sent successfully");
                }

                var res = new MemberInvitationResponse
                {
                    InvitationId = invitation.MemberInvitationId
                };

                return ApiResponse.Success<MemberInvitationResponse>(res);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Something went wrong");
                return ApiResponse.Fail<MemberInvitationResponse>(ResponseCodes.SystemError);
            }
        }
    }
}
