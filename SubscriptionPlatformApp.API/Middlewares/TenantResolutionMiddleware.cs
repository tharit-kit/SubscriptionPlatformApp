using SubscriptionPlatformApp.Application.Abstractions.Persistence;
using SubscriptionPlatformApp.Application.DTOs.Common;

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

            tenantContextAccessor.Current = new TenantContext
            {
                TenantId = tenant.TenantId,
                Slug = tenant.Slug
            };

            await _next(context);
        }
    }
}
