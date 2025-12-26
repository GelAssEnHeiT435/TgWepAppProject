using FlowerBot.src.Core.Interfaces;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;

namespace FlowerBot.src.Core.Services
{
    public class AuthService : IAuthService
    {
        private readonly HashSet<long> _adminIds;
        public AuthService(IConfiguration configuration) =>
            _adminIds = configuration
                .GetSection("TelegramBot:Admins")
                .Get<List<long>>() 
                ?.ToHashSet() ?? new HashSet<long>();

        public string GenerateJwt(long telegramId, TimeSpan expires)
        {
            var claims = new[]
            {
                new Claim("telegramId", telegramId.ToString()),
                new Claim(ClaimTypes.Role, IsAdmin(telegramId) ? "admin" : "user")
            };

            var token = new JwtSecurityToken(
                issuer: AuthOptions.ISSUER,
                audience: AuthOptions.AUDIENCE,
                claims: claims,
                expires: DateTime.UtcNow.Add(expires),
                signingCredentials: new SigningCredentials(
                    AuthOptions.GetSymmetricSecurityKey(),
                    SecurityAlgorithms.HmacSha256)
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private bool IsAdmin(long telegramId) =>
            _adminIds.Contains(telegramId);
    }
}
