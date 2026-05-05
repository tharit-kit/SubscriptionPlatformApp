using Microsoft.IdentityModel.Tokens;
using SubscriptionPlatformApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SubscriptionPlatformApp.Application.Helpers
{
    public static class JwtGenerator
    {
        public static string GenerateJwt(Users user)
        {
            var claims = new[]
            {
                new Claim("userId", user.UserId.ToString()),
                new Claim("email", user.Email)
            };

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes("Z5V4uQWh376cY6XvJJra6czzAzGyEFRRylUwSTIS0wz"));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
