using SubscriptionPlatformApp.Application.Abstractions.Persistence;
using SubscriptionPlatformApp.Application.DTOs.UseCases;
using SubscriptionPlatformApp.Application.DTOs.UseCases.TenantRegistrationUseCase;
using SubscriptionPlatformApp.Domain.Entities;

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

            var newTenant = new Tenants
            {
                TenantId = tenantId,
                TenantName = request.TenantInfo.TenantName,
                BusinessType = request.TenantInfo.BusinessType,
                TenantStatus = "",
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
                AddressType = ""
            };

            var newAdmin = new Users
            {
                UserId = newAdminId,
                Email = request.NewAdmin.Email,
                HashedPassword = "",
                GeneratedSalt = "",
                UserStatus = ""
            };

            var newMembership = new Memberships
            {
                MembershipId = Guid.NewGuid(),
                TenantId = tenantId,
                UserId = newAdminId,
                Role = "Admin",
                MemberStatus = "",
                JoinedAt = DateTime.UtcNow,
            };

            var emailVerification = new EmailVerificationTokens
            {
                EmailVerificationTokenId = Guid.NewGuid(),
                UserId = newAdminId,
                ExpireAt = DateTime.UtcNow.AddMinutes(15),
            };
        }
    }
}
