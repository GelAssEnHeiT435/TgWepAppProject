using FlowerBot.src.Core.Interfaces;
using FlowerBot.src.Options;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace FlowerBot.src.Core.Services
{
    public class AuthService : IAuthService
    {
        private readonly HashSet<long> _adminIds;
        private readonly JwtOptions _jwtOptions;
        public AuthService(
            IOptions<JwtOptions> jwtOptions, 
            IOptions<TelegramOptions> telegramOptions)
        {
            _jwtOptions = jwtOptions.Value;
            _adminIds = telegramOptions.Value.Admins?.ToHashSet() ?? new HashSet<long>();
        }
            

        public string GenerateJwt(long telegramId, TimeSpan expires)
        {
            var claims = new[]
            {
                new Claim("telegramId", telegramId.ToString()),
                new Claim(ClaimTypes.Role, IsAdmin(telegramId) ? "admin" : "user")
            };

            var token = new JwtSecurityToken(
                issuer: _jwtOptions.Issuer,
                audience: _jwtOptions.Audience,
                claims: claims,
                expires: DateTime.UtcNow.Add(expires),
                signingCredentials: new SigningCredentials(
                    GetSymmetricSecurityKey(_jwtOptions.Key),
                    SecurityAlgorithms.HmacSha256)
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private bool IsAdmin(long telegramId) =>
            _adminIds.Contains(telegramId);

        public static SymmetricSecurityKey GetSymmetricSecurityKey(string key) =>
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
    }
}
