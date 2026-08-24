using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using SubscriptionPlatformApp.Application.Abstractions.Persistence;
using SubscriptionPlatformApp.Application.Abstractions.Providers;
using SubscriptionPlatformApp.Application.Abstractions.Repositories;
using SubscriptionPlatformApp.Application.Abstractions.Services;
using SubscriptionPlatformApp.Application.Abstractions.UseCases;
using SubscriptionPlatformApp.Application.Helpers.AppSettings;
using SubscriptionPlatformApp.Application.UseCases;
using SubscriptionPlatformApp.Application.Utils.Constants;
using SubscriptionPlatformApp.Domain.Enums;
using SubscriptionPlatformApp.Infrastructure.Persistence;
using SubscriptionPlatformApp.Infrastructure.Providers;
using SubscriptionPlatformApp.Infrastructure.Repositories;
using SubscriptionPlatformApp.Infrastructure.Services;
using System.Text;

namespace SubscriptionPlatformApp.Infrastructure.Configurations
{
    public static class ConfigurationExtensions
    {
        public static IServiceCollection AddInfrastructure(
            this IServiceCollection services, IConfiguration config)
        {
            var connectionString = config.GetConnectionString("DefaultConnection");

            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(connectionString, sqlOptions =>
                {
                    sqlOptions.EnableRetryOnFailure();
                }));

            services.AddCors(options =>
             {
                 options.AddPolicy("AllowFrontend", policy =>
                 {
                     policy
                         .WithOrigins("http://localhost:5173")
                         .AllowAnyHeader()
                         .AllowAnyMethod()
                         .AllowCredentials();
                 });
             });

            services.Configure<RouteOptions>(options =>
            {
                options.LowercaseUrls = true;
                options.LowercaseQueryStrings = true; // Optional: also lowercase query parameters
            });

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ValidateIssuerSigningKey = true,
                        ValidateLifetime = true,
                        IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes("Z5V4uQWh376cY6XvJJra6czzAzGyEFRRylUwSTIS0wz"))
                    };
                });

            services.AddAuthorization(options =>
            {
                options.AddPolicy(AuthorizationPolicyConstants.AdminOnly, policy =>
                {
                    policy.RequireAssertion(context =>
                    {
                        if (context.Resource is not HttpContext httpContext)
                        {
                            return false;
                        }

                        var tenantContextAccessor =
                            httpContext.RequestServices
                                .GetRequiredService<ITenantContextAccessor>();

                        return tenantContextAccessor.Current?.Role == MembershipRole.Admin.ToString();
                    });
                });
            });

            services
                .AddAndValidate<SmtpSetting>(config)
                .AddAndValidate<FrontendSetting>(config);

            return services;
        }

        public static IServiceCollection AddApplication(
            this IServiceCollection services)
        {
            // Add Services for Repositories
            services.AddScoped<ITenantRepository, TenantRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IAddressRepository, AddressRepository>();
            services.AddScoped<IEmailVerificationTokenRepository, EmailVerificationTokenRepository>();
            services.AddScoped<IMemberInvitationRepository, MemberInvitationRepository>();
            services.AddScoped<IMembershipRepository, MembershipRepository>();
            services.AddScoped<IPaymentRepository, PaymentRepository>();
            services.AddScoped<ISubscriptionRepository, SubscriptionRepository>();

            // Add Services for usecases
            services.AddScoped<ITenantRegistrationUseCase, TenantRegistrationUseCase>();
            services.AddScoped<IEmailVerificationUseCase, EmailVerificationUseCase>();
            services.AddScoped<IResendVerificationEmailUseCase, ResendVerificationEmailUseCase>();
            services.AddScoped<ILoginUseCase, LoginUseCase>();
            services.AddScoped<IGetMemberUseCase, GetMemberUseCase>();
            services.AddScoped<IMemberInvitaionUseCase, MemberInvitaionUseCase>();
            services.AddScoped<IVerifyMemberInvitationUseCase, VerifyMemberInvitationUseCase>();
            services.AddScoped<IAcceptMemberInvitationUseCase, AcceptMemberInvitationUseCase>();

            // Add Services for providers
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<ISmtpProvider, SmtpProvider>();

            services.AddScoped<ITenantContextAccessor, TenantContextAccessor>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<ICurrentUserService, CurrentUserService>();

            services.AddSingleton<ISecureTokenGenerator, SecureTokenGenerator>();

            return services;
        }

        private static IServiceCollection AddAndValidate<T>(
            this IServiceCollection services,
            IConfiguration configuration)
            where T : class
        {
            var sectionName = typeof(T)
                .GetField("SectionName")?
                .GetValue(null) as string
                ?? throw new InvalidOperationException(
                    $"{typeof(T).Name} must define SectionName");

            services
                .AddOptions<T>()
                .Bind(configuration.GetSection(sectionName))
                .ValidateDataAnnotations()
                .ValidateOnStart();

            return services;
        }
    }
}
