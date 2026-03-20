using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SubscriptionPlatformApp.Application.Abstractions.Repositories;
using SubscriptionPlatformApp.Application.Helpers.AppSettings;
using SubscriptionPlatformApp.Infrastructure.Persistence;
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
                .AddAndValidate<SmtpSetting>(config);

            //services.Configure<PositionOptions>(
            //    config.GetSection(PositionOptions.Position));
            //services.Configure<ColorOptions>(
            //    config.GetSection(ColorOptions.Color));

            return services;
        }

        public static IServiceCollection AddApplication(
            this IServiceCollection services)
        {
            // Add Services for Repo
            services.AddScoped<ITenantRepository, TenantRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IAddressRepository, AddressRepository>();
            services.AddScoped<IEmailVerificationTokenRepository, EmailVerificationTokenRepository>();
            services.AddScoped<IMemberInvitationRepository, MemberInvitationRepository>();
            services.AddScoped<IMembershipRepository, MembershipRepository>();
            services.AddScoped<IPaymentRepository, PaymentRepository>();
            services.AddScoped<ISubscriptionRepository, SubscriptionRepository>();

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
