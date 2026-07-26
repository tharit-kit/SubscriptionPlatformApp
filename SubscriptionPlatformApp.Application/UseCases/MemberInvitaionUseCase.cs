using Microsoft.Extensions.Logging;
using SubscriptionPlatformApp.Application.Abstractions.Persistence;
using SubscriptionPlatformApp.Application.Abstractions.Services;
using SubscriptionPlatformApp.Application.DTOs.UseCases;
using SubscriptionPlatformApp.Application.DTOs.UseCases.GetMemberUseCase;
using SubscriptionPlatformApp.Application.DTOs.UseCases.MemberInvitationUseCase;
using SubscriptionPlatformApp.Application.Utils.Constants;
using SubscriptionPlatformApp.Application.Utils.Response;
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
        }

        public async Task<ApiResponse<MemberInvitationResponse>> ExecuteAsync(MemberInvitationRequest request, CancellationToken ct)
        {
            try
            {
                if (!_currentUserService.IsAuthenticated)
                {

                }

                if (_currentUserService.Role != RoleConstants.ADMIN_ROLE)
                {

                }

                var membership = await _unitOfWork.Membership.FindByUserId(_currentUserService.UserId, true, ct);
                if (membership == null)
                {

                }


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
