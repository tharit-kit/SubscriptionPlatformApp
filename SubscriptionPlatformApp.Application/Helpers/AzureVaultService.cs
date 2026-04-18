using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using System;
using System.Collections.Generic;
using System.Text;

namespace SubscriptionPlatformApp.Application.Helpers
{
    public class AzureVaultService
    {
        public static async Task<KeyVaultSecret> VaultSecretReader(string secretName)
        {
            try
            {
                var keyVaultName = "portfolio-subscription";
                var kvUri = $"https://{keyVaultName}.vault.azure.net";

                var client = new SecretClient(new Uri(kvUri), new DefaultAzureCredential());

                var secret = await client.GetSecretAsync(secretName);

                return secret;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
    }
}
