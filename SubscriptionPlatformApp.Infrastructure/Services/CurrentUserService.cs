using Microsoft.AspNetCore.Http;
using SubscriptionPlatformApp.Application.Abstractions.Services;
using System.Security.Claims;

namespace SubscriptionPlatformApp.Infrastructure.Services
{
    public sealed class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        private ClaimsPrincipal? User =>
            _httpContextAccessor.HttpContext?.User;

        public bool IsAuthenticated =>
            User?.Identity?.IsAuthenticated == true;

        public Guid UserId =>
            GetRequiredGuidClaim(ClaimTypes.NameIdentifier);

        public Guid TenantId =>
            GetRequiredGuidClaim("tenant_id");

        public string? Role =>
            User?.FindFirstValue(ClaimTypes.Role);

        private Guid GetRequiredGuidClaim(string claimType)
        {
            var value = User?.FindFirstValue(claimType);

            if (!Guid.TryParse(value, out var id))
            {
                throw new UnauthorizedAccessException(
                    $"The authenticated user does not contain a valid '{claimType}' claim.");
            }

            return id;
        }
    }
}
