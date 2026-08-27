using SubscriptionPlatformApp.Application.Abstractions.Persistence;
using SubscriptionPlatformApp.Application.Abstractions.UseCases;
using SubscriptionPlatformApp.Application.DTOs.UseCases;
using SubscriptionPlatformApp.Application.DTOs.UseCases.GetMemberUseCase;
using SubscriptionPlatformApp.Application.Utils.Response;

namespace SubscriptionPlatformApp.Application.UseCases
{
    public class GetMemberUseCase : IGetMemberUseCase
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetMemberUseCase(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ApiResponse<GetMemberResponse>> ExecuteAsync(CancellationToken ct)
        {
            try
            {
                var members = await _unitOfWork.Membership.GetMembershipByTenantId(ct);
                var memberInvitations = await _unitOfWork.MemberInvitation.GetMemberInvitationsByTenantId(ct);

                var data = members.Select(x => new MemberInfo
                {
                    FullName = x.User.FullName,
                    Email = x.User.Email,
                    Role = x.Role ?? string.Empty,
                    MemberStatus = x.MemberStatus.ToString(),
                    JoinAt = x.JoinedAt.ToString()
                }).ToList();

                data.AddRange(memberInvitations.Select(x => new MemberInfo
                {
                    FullName = string.Empty,
                    Email = x.InvitedEmail,
                    Role = x.Role,
                    MemberStatus = x.InvitationStatus.ToString(),
                    JoinAt = null
                }));

                var res = new GetMemberResponse
                {
                    MemberInfos = data,
                };

                return ApiResponse.Success<GetMemberResponse>(res);
            }
            catch (Exception ex)
            {
                return ApiResponse.Fail<GetMemberResponse>(ResponseCodes.SystemError);
            }
        }
    }
}
