using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SubscriptionPlatformApp.Application.Abstractions.Persistence;
using SubscriptionPlatformApp.Application.Abstractions.Providers;
using SubscriptionPlatformApp.Application.Abstractions.Repositories;
using SubscriptionPlatformApp.Application.Abstractions.UseCases;
using SubscriptionPlatformApp.Application.Helpers.AppSettings;
using SubscriptionPlatformApp.Application.UseCases;
using SubscriptionPlatformApp.Infrastructure.Persistence;
using SubscriptionPlatformApp.Infrastructure.Providers;
using SubscriptionPlatformApp.Infrastructure.Repositories;

namespace SubscriptionPlatformApp.Infrastructure.Configurations
{
    public static class ConfigurationExtensions
    {
        public static IServiceCollection AddInfrastructure(
            this IServiceCollection services, IConfiguration config)
        {
            var connectionString = config.GetConnectionString("DefaultConnection");

            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(connectionString));

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

            // Add Services for providers
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<ISmtpProvider, SmtpProvider>();

            services.AddScoped<ITenantContextAccessor, TenantContextAccessor>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();

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
