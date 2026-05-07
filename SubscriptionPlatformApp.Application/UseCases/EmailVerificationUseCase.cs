using SubscriptionPlatformApp.Application.Abstractions.Persistence;
using SubscriptionPlatformApp.Application.Abstractions.UseCases;
using SubscriptionPlatformApp.Application.DTOs.UseCases;
using SubscriptionPlatformApp.Application.DTOs.UseCases.UserVerificationUseCase;
using SubscriptionPlatformApp.Application.Utils.Response;
using SubscriptionPlatformApp.Domain.Enums;

namespace SubscriptionPlatformApp.Application.UseCases
{
    public class EmailVerificationUseCase : IEmailVerificationUseCase
    {
        private readonly IUnitOfWork _unitOfWork;

        public EmailVerificationUseCase(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ApiResponse<EmailVerificationResponse>> ExecuteAsync(Guid tokenId, CancellationToken ct)
        {
            try
            {
                var token = await _unitOfWork.EmailVerificationToken.FindByIdAsync(tokenId, ct);

                if (token == null)
                {
                    return ApiResponse.Fail<EmailVerificationResponse>(ResponseCodes.VerificationTokenNotFound);
                }

                var data = new EmailVerificationResponse()
                {
                    UserId = token.UserId,
                    TenantId = token.TenantId,
                };

                if (token.ExpireAt < DateTime.UtcNow)
                {
                    return ApiResponse.Fail<EmailVerificationResponse>(ResponseCodes.VerificationTokenExpired, data);
                }

                var user = token.User;
                if (user == null)
                {
                    return ApiResponse.Fail<EmailVerificationResponse>(ResponseCodes.UserNotFound);
                }

                var membership = await _unitOfWork.Membership.FindByTenantIdAndUserIdAsync(token.TenantId, token.UserId, ct);
                if (membership == null)
                {
                    return ApiResponse.Fail<EmailVerificationResponse>(ResponseCodes.MembershipNotFound);
                }

                var tenant = await _unitOfWork.Tenant.FindByIdAsync(token.TenantId, ct);
                if (tenant == null)
                {
                    return ApiResponse.Fail<EmailVerificationResponse>(ResponseCodes.TenantNotFound);
                }

                if (user.UserStatus == UserStatus.Pending || membership.MemberStatus == MemberStatus.Pending || tenant.TenantStatus == TenantStatus.Pending)
                {
                    // email not verified
                    user.UserStatus = UserStatus.Active;
                    membership.MemberStatus = MemberStatus.Active;
                    tenant.TenantStatus = TenantStatus.Active;

                    _unitOfWork.User.Update(user);
                    _unitOfWork.Membership.Update(membership);
                    _unitOfWork.Tenant.Update(tenant);

                    await _unitOfWork.SaveChangesAsync(ct);
                }
                else if (user.UserStatus == UserStatus.Active && membership.MemberStatus == MemberStatus.Active && tenant.TenantStatus == TenantStatus.Active)
                {
                    // email already verified
                    return ApiResponse.Fail<EmailVerificationResponse>(ResponseCodes.EmailAlreadyVerified);
                }
                else
                {
                    // email rejected, not allow to verified
                    return ApiResponse.Fail<EmailVerificationResponse>(ResponseCodes.UserRejected);
                }

                return ApiResponse.Success<EmailVerificationResponse>(data);
            }
            catch (Exception ex)
            {
                return ApiResponse.Fail<EmailVerificationResponse>(ResponseCodes.SystemError);
            }
        }
    }
}
