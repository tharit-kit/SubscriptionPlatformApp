using System;
using System.Collections.Generic;
using System.Text;

namespace SubscriptionPlatformApp.Application.Abstractions.Services
{
    public interface ISecureTokenGenerator
    {
        GeneratedToken Generate();

        string Hash(string token);

        bool Verify(string token, string expectedHash);
    }

    public sealed record GeneratedToken(
        string Value,
        string Hash);
}
