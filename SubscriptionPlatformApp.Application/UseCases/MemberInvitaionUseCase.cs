using Microsoft.Extensions.Logging;
using SubscriptionPlatformApp.Application.Abstractions.Persistence;
using SubscriptionPlatformApp.Application.Abstractions.Services;
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
    public class MemberInvitaionUseCase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;
        private readonly ILogger<MemberInvitaionUseCase> _logger;

        public MemberInvitaionUseCase(IUnitOfWork unitOfWork, ICurrentUserService currentUserService, ILogger<MemberInvitaionUseCase> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _currentUserService = currentUserService;
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

                var invitation = new MemberInvitations
                {
                    MemberInvitationId = Guid.NewGuid(),
                    TenantId = _currentUserService.TenantId,
                    InvitedEmail = request.Email,
                    Role = request.Role,
                    HashedToken = 
                    CreatedBy = _currentUserService.UserId,
                    CreatedAt = DateTime.UtcNow,
                    InvitationStatus = InvitationStatus.Invited,
                    ExpiresAt = DateTime.UtcNow.AddMinutes(15),
                };
                //var members = await _unitOfWork.Membership.GetMembershipByTenantId(ct);

                //var data = members.Select(x => new MemberInfo
                //{
                //    FullName = x.User.FullName,
                //    Role = x.Role ?? string.Empty,
                //    MemberStatus = x.MemberStatus.ToString(),
                //    JoinAt = x.JoinedAt.ToString()
                //}).ToList();

                //var res = new GetMemberResponse
                //{
                //    MemberInfos = data,
                //};

                //return ApiResponse.Success<GetMemberResponse>(res);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Something went wrong");
                return ApiResponse.Fail<MemberInvitationResponse>(ResponseCodes.SystemError);
            }
        }
    }
}
