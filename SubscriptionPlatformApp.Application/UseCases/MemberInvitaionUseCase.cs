using Microsoft.Extensions.Logging;
using SubscriptionPlatformApp.Application.Abstractions.Persistence;
using SubscriptionPlatformApp.Application.DTOs.UseCases;
using SubscriptionPlatformApp.Application.DTOs.UseCases.GetMemberUseCase;
using SubscriptionPlatformApp.Application.DTOs.UseCases.MemberInvitationUseCase;
using SubscriptionPlatformApp.Application.Utils.Response;
using System;
using System.Collections.Generic;
using System.Text;

namespace SubscriptionPlatformApp.Application.UseCases
{
    public class MemberInvitaionUseCase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<MemberInvitaionUseCase> _logger;

        public MemberInvitaionUseCase(IUnitOfWork unitOfWork, ILogger<MemberInvitaionUseCase> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<ApiResponse<MemberInvitationResponse>> ExecuteAsync(MemberInvitationRequest request, CancellationToken ct)
        {
            try
            {
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
