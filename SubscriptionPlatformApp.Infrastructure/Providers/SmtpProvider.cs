using Microsoft.Extensions.Options;
using SubscriptionPlatformApp.Application.Abstractions.Providers;
using SubscriptionPlatformApp.Application.DTOs.Providers.SmtpProvider;
using SubscriptionPlatformApp.Application.Helpers;
using SubscriptionPlatformApp.Application.Helpers.AppSettings;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace SubscriptionPlatformApp.Infrastructure.Providers
{
    public class SmtpProvider : ISmtpProvider
    {
        private readonly HttpClient _httpClient;
        private readonly SmtpSetting _smtpSettings;

        public SmtpProvider(
            HttpClient httpClient,
            IOptions<SmtpSetting> smtpSettings)
        {
            _httpClient = httpClient;
            _smtpSettings = smtpSettings.Value;
        }

        public async Task<bool> SendingEmailAsync(SendingEmailRequest message)
        {
            var apiKey = await AzureVaultService.VaultSecretReader("BrevoApiKey");
            _httpClient.BaseAddress = new Uri(_smtpSettings.BaseUrl);
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            _httpClient.DefaultRequestHeaders.Add("api-key", apiKey.Value);

            var json = JsonSerializer.Serialize(message, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });
            using var content = new StringContent(json, Encoding.UTF8, new MediaTypeWithQualityHeaderValue("application/json"));
            using var res = await _httpClient.PostAsync("v3/smtp/email", content);

            var body = await res.Content.ReadAsStringAsync();

            if (!res.IsSuccessStatusCode)
            {
                return false;
            }
            return true;
        }
    }
}
