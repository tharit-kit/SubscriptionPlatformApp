using Microsoft.AspNetCore.Http;
using SubscriptionPlatformApp.Application.Abstractions.Services;
using System.Security.Claims;

namespace SubscriptionPlatformApp.Infrastructure.Services
{
    public sealed class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentUserService(
            IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public Guid? UserId
        {
            get
            {
                var value = _httpContextAccessor
                    .HttpContext?
                    .User
                    .FindFirst(ClaimTypes.NameIdentifier)?
                    .Value;

                return Guid.TryParse(value, out var userId)
                    ? userId
                    : null;
            }
        }

        public string? Email =>
            _httpContextAccessor
                .HttpContext?
                .User
                .FindFirst(ClaimTypes.Email)?
                .Value;

        public bool IsAuthenticated =>
            _httpContextAccessor
                .HttpContext?
                .User
                .Identity?
                .IsAuthenticated == true;
    }
}
