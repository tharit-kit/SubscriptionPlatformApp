using Microsoft.Extensions.Logging;
using SubscriptionPlatformApp.Application.Abstractions.Persistence;
using SubscriptionPlatformApp.Application.Abstractions.UseCases;
using SubscriptionPlatformApp.Application.DTOs.UseCases;
using SubscriptionPlatformApp.Application.DTOs.UseCases.LoginUseCase;
using SubscriptionPlatformApp.Application.Helpers;
using SubscriptionPlatformApp.Application.Utils.Response;

namespace SubscriptionPlatformApp.Application.UseCases
{
    public class LoginUseCase : ILoginUseCase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<LoginUseCase> _logger;
        public LoginUseCase(IUnitOfWork unitOfWork, ILogger<LoginUseCase> logger)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        public async Task<ApiResponse<LoginResponse>> ExecuteAsync(LoginRequest request, CancellationToken ct)
        {
            try
            {
                var user = await _unitOfWork.User.FindByEmail(request.Email, ct);
                if (user == null)
                {
                    return ApiResponse.Fail<LoginResponse>(ResponseCodes.UserNotFound);
                }

                if (user.GeneratedSalt == null)
                {
                    return ApiResponse.Fail<LoginResponse>(ResponseCodes.Unauthorized);
                }

                var hashedPassword = PasswordHasher.GenerateHash(request.Password, user.GeneratedSalt);
                if (hashedPassword != user.HashedPassword)
                {
                    return ApiResponse.Fail<LoginResponse>(ResponseCodes.Unauthorized);
                }

                var memberships = await _unitOfWork.Membership.FindByUserId(user.UserId, false, ct);
                var token = JwtGenerator.GenerateJwt(user);

                var data = new LoginResponse()
                {
                    AccessToken = token,
                    Memberships = [.. memberships.Select(x => new MembershipInfo
                    {
                        TenantName = x.Tenant.TenantName,
                        Slug = x.Tenant.Slug,
                        Role = x.Role ?? string.Empty
                    })],
                };

                return ApiResponse.Success(data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unable to login");
                return ApiResponse.Fail<LoginResponse>(ResponseCodes.SystemError);
            }
        }
    }
}
