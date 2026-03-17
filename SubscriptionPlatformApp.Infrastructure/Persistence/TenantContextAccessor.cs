using Microsoft.AspNetCore.Http;
using SubscriptionPlatformApp.Application.Abstractions.Persistence;
using SubscriptionPlatformApp.Application.DTOs.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace SubscriptionPlatformApp.Infrastructure.Persistence
{
    public class TenantContextAccessor : ITenantContextAccessor
    {
        private const string TenantContextKey = "__TENANT_CONTEXT__";
        private readonly IHttpContextAccessor _httpContextAccessor;

        public TenantContextAccessor(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public TenantContext? Current
        {
            get
            {
                var httpContext = _httpContextAccessor.HttpContext;
                if (httpContext == null) return null;

                return httpContext.Items.TryGetValue(TenantContextKey, out var value)
                    ? value as TenantContext
                    : null;
            }
            set
            {
                var httpContext = _httpContextAccessor.HttpContext;
                if (httpContext == null) return;

                httpContext.Items[TenantContextKey] = value!;
            }
        }
    }
}
