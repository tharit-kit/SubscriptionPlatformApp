using SubscriptionPlatformApp.Application.Abstractions.Persistence;
using SubscriptionPlatformApp.Application.DTOs.Common;
using SubscriptionPlatformApp.Domain.Entities;

namespace SubscriptionPlatformApp.API.Middlewares
{
    public sealed class TenantResolutionMiddleware
    {
        private readonly RequestDelegate _next;

        public TenantResolutionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(
            HttpContext context,
            IUnitOfWork unitOfWork,
            ITenantContextAccessor tenantContextAccessor)
        {
            var slug = context.Request.Headers["X-Tenant-Slug"].FirstOrDefault();

            if (string.IsNullOrWhiteSpace(slug))
            {
                await _next(context);
                return;
            }

            slug = slug.Trim().ToLowerInvariant();

            var tenant = await unitOfWork.Tenant.FindBySlugAsync(slug, context.RequestAborted);

            if (tenant == null)
            {
                context.Response.StatusCode = StatusCodes.Status404NotFound;
                await context.Response.WriteAsync("Tenant not found.");
                return;
            }

            var userIdClaim = context.User.FindFirst("userId")?.Value;

            if (userIdClaim == null)
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsync("User not authorized.");
                return;
            }

            var userId = Guid.Parse(userIdClaim);

            var membership = await unitOfWork.Membership.FindByTenantIdAndUserIdAsync(tenant.TenantId, userId, context.RequestAborted);

            if (membership == null)
            {
                context.Response.StatusCode = StatusCodes.Status403Forbidden;
                return;
            }

            tenantContextAccessor.Current = new TenantContext
            {
                TenantId = tenant.TenantId,
                Slug = tenant.Slug,
                UserId = userId,
                Role = membership.Role ?? string.Empty
            };

            await _next(context);
        }
    }
}
