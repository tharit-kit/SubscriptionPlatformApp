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

                if (token.ExpireAt < DateTime.UtcNow)
                {
                    return ApiResponse.Fail<EmailVerificationResponse>(ResponseCodes.VerificationTokenExpired);
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

                if (user.UserStatus == UserStatus.Pending)
                {
                    // email not verified
                    user.UserStatus = UserStatus.Active;
                    membership.MemberStatus = MemberStatus.Active;

                    _unitOfWork.User.Update(user);
                    _unitOfWork.Membership.Update(membership);
                    await _unitOfWork.SaveChangesAsync(ct);
                }
                else if (user.UserStatus == UserStatus.Active)
                {
                    // email already verified
                    return ApiResponse.Fail<EmailVerificationResponse>(ResponseCodes.EmailAlreadyVerified);
                }
                else
                {
                    // email rejected, not allow to verified
                    return ApiResponse.Fail<EmailVerificationResponse>(ResponseCodes.UserRejected);
                }

                var data = new EmailVerificationResponse()
                {
                    UserId = token.UserId,
                    TenantId = token.TenantId,
                };
                return ApiResponse.Success<EmailVerificationResponse>(data);
            }
            catch (Exception ex)
            {
                return ApiResponse.Fail<EmailVerificationResponse>(ResponseCodes.SystemError);
            }
        }
    }
}
