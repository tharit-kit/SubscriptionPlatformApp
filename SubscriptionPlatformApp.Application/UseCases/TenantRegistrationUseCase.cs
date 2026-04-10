using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SubscriptionPlatformApp.Application.Abstractions.Persistence;
using SubscriptionPlatformApp.Application.Abstractions.Providers;
using SubscriptionPlatformApp.Application.Abstractions.UseCases;
using SubscriptionPlatformApp.Application.DTOs.UseCases;
using SubscriptionPlatformApp.Application.DTOs.UseCases.TenantRegistrationUseCase;
using SubscriptionPlatformApp.Application.Helpers;
using SubscriptionPlatformApp.Application.Helpers.AppSettings;
using SubscriptionPlatformApp.Application.Utils.Constants;
using SubscriptionPlatformApp.Application.Utils.Response;
using SubscriptionPlatformApp.Domain.Entities;
using SubscriptionPlatformApp.Domain.Enums;
using System.Buffers.Text;

namespace SubscriptionPlatformApp.Application.UseCases
{
    public class TenantRegistrationUseCase : ITenantRegistrationUseCase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEmailBaseProvider _emailBaseProvider;
        private readonly FrontendSetting _frontendSetting;
        private readonly ILogger<TenantRegistrationUseCase> _logger;

        public TenantRegistrationUseCase(IUnitOfWork unitOfWork, IEmailBaseProvider emailBaseProvider, IOptions<FrontendSetting> frontendSetting, ILogger<TenantRegistrationUseCase> logger)
        {
            _unitOfWork = unitOfWork;
            _emailBaseProvider = emailBaseProvider;
            _frontendSetting = frontendSetting.Value;
            _logger = logger;
        }

        public async Task<ApiResponse<TenantRegistrationResponse>> ExecuteAsync(TenantRegistrationRequest request, CancellationToken ct)
        {
            var newTenantId = Guid.NewGuid();
            var newTenantAddressId = Guid.NewGuid();
            var newAdminId = Guid.NewGuid();
            var verificationTokenId = Guid.NewGuid();
            var now = DateTime.UtcNow;

            try
            {
                var newTenant = new Tenants
                {
                    TenantId = newTenantId,
                    TenantName = request.TenantInfo.TenantName,
                    BusinessType = request.TenantInfo.BusinessType,
                    TenantStatus = TenantStatus.Pending,
                    TenantAddressId = newTenantAddressId,
                    Slug = request.TenantInfo.Subdomain
                };
                await _unitOfWork.Tenant.AddAsync(newTenant, ct);

                var newTenantAddress = new Addresses
                {
                    AddressId = newTenantAddressId,
                    Address1 = request.TenantAddress.Address1,
                    Address2 = request.TenantAddress.Address2,
                    District = request.TenantAddress.District,
                    SubDistrict = request.TenantAddress.SubDistrict,
                    Province = request.TenantAddress.Province,
                    Zipcode = request.TenantAddress.Zipcode,
                    AddressType = AddressType.Workplace
                };
                await _unitOfWork.Address.AddAsync(newTenantAddress, ct);

                var generatedSalt = PasswordHasher.GenerateSalt();
                var hashedPassword = PasswordHasher.GenerateHash(request.NewAdmin.ConfirmPassword, generatedSalt);

                var newAdmin = new Users
                {
                    UserId = newAdminId,
                    Email = request.NewAdmin.Email,
                    FullName = request.NewAdmin.FullName,
                    HashedPassword = hashedPassword,
                    GeneratedSalt = generatedSalt,
                    UserStatus = UserStatus.Pending
                };
                await _unitOfWork.User.AddAsync(newAdmin, ct);

                var newMembership = new Memberships
                {
                    MembershipId = Guid.NewGuid(),
                    TenantId = newTenantId,
                    UserId = newAdminId,
                    Role = RoleConstants.ADMIN_ROLE,
                    MemberStatus = MemberStatus.Pending,
                    JoinedAt = now,
                };
                await _unitOfWork.Membership.AddAsync(newMembership, ct);

                var emailVerification = new EmailVerificationTokens
                {
                    EmailVerificationTokenId = verificationTokenId,
                    UserId = newAdminId,
                    ExpireAt = now.AddMinutes(15),
                };
                await _unitOfWork.EmailVerificationToken.AddAsync(emailVerification, ct);

                await _unitOfWork.SaveChangesAsync(ct);

                var isSendEmail = await SendUserVerificationEmailAsync(request.NewAdmin.Email, request.NewAdmin.FullName, verificationTokenId, ct);
                if (isSendEmail)
                {
                    _logger.LogInformation("User verification email sent successfully");
                }
                var data = new TenantRegistrationResponse()
                {
                    TenantId = newTenantId,
                    AdminId = newAdminId,
                };

                return new ApiResponse<TenantRegistrationResponse>()
                {
                    ResponseCode = ResponseCodes.Success.Code,
                    ResponseDescription = ResponseCodes.Success.Description,
                    Data = data
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Cannot register a new tenant and admin");
                return new ApiResponse<TenantRegistrationResponse>()
                {
                    ResponseCode = ResponseCodes.SystemError.Code,
                    ResponseDescription = ResponseCodes.SystemError.Description
                };
            }
        }

        private async Task<bool> SendUserVerificationEmailAsync(
            string email,
            string fullName,
            Guid verificationToken,
            CancellationToken ct = default)
        {
            var template = EmailTemplateReader.GetEmailTemplate("UserVerificationEmailTemplate.html");
            var verificationLink = $"{_frontendSetting.BaseUrl}/verify-email?token={verificationToken}";

            var htmlContent = template
                .Replace("{{VERIFICATION_LINK}}", verificationLink);

            return await _emailBaseProvider.SendEmailAsync(
                email,
                fullName,
                "Email Verification",
                htmlContent,
                "verification",
                ct
            );
        }
    }
}
