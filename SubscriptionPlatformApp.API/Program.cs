using SubscriptionPlatformApp.API.Middlewares;
using SubscriptionPlatformApp.Infrastructure.Configurations;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddHttpContextAccessor();
builder.Services.AddHttpClient();
builder.Services
    .AddInfrastructure(builder.Configuration)
    .AddApplication();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddCors(options =>
{
    options.AddPolicy("Frontend", policy =>
    {
        policy
            .SetIsOriginAllowed(origin =>
            {
                if (string.IsNullOrWhiteSpace(origin)) return false;

                var uri = new Uri(origin);
                var host = uri.Host.ToLowerInvariant();

                return host == "subscriptionplatform.com"
                    || host.EndsWith(".vercel.app");
            })
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
});

builder.Services.Configure<RouteOptions>(options =>
{
    options.LowercaseUrls = true;
    options.LowercaseQueryStrings = true; // Optional: also lowercase query parameters
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseCors("Frontend");

app.UseMiddleware<TenantResolutionMiddleware>();

app.UseAuthorization();

app.MapControllers();

app.Run();
