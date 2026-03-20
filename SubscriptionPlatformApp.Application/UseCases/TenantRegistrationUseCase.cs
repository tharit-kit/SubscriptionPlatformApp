using SubscriptionPlatformApp.Application.Abstractions.Persistence;
using SubscriptionPlatformApp.Application.DTOs.UseCases;
using SubscriptionPlatformApp.Application.DTOs.UseCases.TenantRegistrationUseCase;
using SubscriptionPlatformApp.Application.Helpers;
using SubscriptionPlatformApp.Application.Utils.Constants;
using SubscriptionPlatformApp.Domain.Entities;
using SubscriptionPlatformApp.Domain.Enums;

namespace SubscriptionPlatformApp.Application.UseCases
{
    public class TenantRegistrationUseCase : ITenantRegistrationUseCase
    {
        private readonly IUnitOfWork _unitOfWork;

        public TenantRegistrationUseCase(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ApiResponse<TenantRegistrationResponse>> ExecuteAsync(TenantRegistrationRequest request, CancellationToken ct)
        {
            var tenantId = Guid.NewGuid();
            var addressId = Guid.NewGuid();
            var newAdminId = Guid.NewGuid();
            var now = DateTime.UtcNow;

            var newTenant = new Tenants
            {
                TenantId = tenantId,
                TenantName = request.TenantInfo.TenantName,
                BusinessType = request.TenantInfo.BusinessType,
                TenantStatus = TenantStatus.Pending,
                TenantAddressId = addressId,
                Slug = request.TenantInfo.Subdomain
            };

            var newTenantAddress = new Addresses
            {
                AddressId = addressId,
                Address1 = request.TenantAddress.Address1,
                Address2 = request.TenantAddress.Address2,
                District = request.TenantAddress.District,
                SubDistrict = request.TenantAddress.SubDistrict,
                Province = request.TenantAddress.Province,
                Zipcode = request.TenantAddress.Zipcode,
                AddressType = AddressType.Workplace
            };

            var generatedSalt = PasswordHasher.GenerateSalt();
            var hashedPassword = PasswordHasher.GenerateHash(request.NewAdmin.ConfirmPassword, generatedSalt);

            var newAdmin = new Users
            {
                UserId = newAdminId,
                Email = request.NewAdmin.Email,
                HashedPassword = hashedPassword,
                GeneratedSalt = generatedSalt,
                UserStatus = UserStatus.Pending
            };

            var newMembership = new Memberships
            {
                MembershipId = Guid.NewGuid(),
                TenantId = tenantId,
                UserId = newAdminId,
                Role = RoleConstants.ADMIN_ROLE,
                MemberStatus = MemberStatus.Pending,
                JoinedAt = now,
            };

            var emailVerification = new EmailVerificationTokens
            {
                EmailVerificationTokenId = Guid.NewGuid(),
                UserId = newAdminId,
                ExpireAt = now.AddMinutes(15),
            };

            await _unitOfWork.SaveChangesAsync(ct);


        }
    }
}
