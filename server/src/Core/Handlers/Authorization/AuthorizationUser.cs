using FlowerBot.src.Core.Interfaces;
using FlowerBot.src.Data;
using FlowerBot.src.Data.Models.Common;
using FlowerBot.src.Data.Models.Database;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace FlowerBot.src.Core.Handlers.Authorization
{
    public record class AuthorizationUserCommand(long? TelegramId): IRequest<TokensResult?>;

    public class AuthorizationUserHandler : IRequestHandler<AuthorizationUserCommand, TokensResult?>
    {
        private readonly ApplicationContext _context;
        private readonly IAuthService _authService;

        public AuthorizationUserHandler(ApplicationContext context, IAuthService authService)
        {
            _context = context;
            _authService = authService;
        }

        public async Task<TokensResult?> Handle(AuthorizationUserCommand request, CancellationToken cancellationToken)
        {
            if (!request.TelegramId.HasValue || request.TelegramId < 0) return null;

            string accessToken = _authService.GenerateJwt(request.TelegramId.Value, TimeSpan.FromMinutes(15));

            RefreshToken refreshToken = new RefreshToken
            {
                Id = Guid.NewGuid(),
                TelegramId = request.TelegramId.Value,
                ExpiresAt = DateTime.UtcNow.AddDays(1)
            };

            _context.RefreshTokens.Add(refreshToken);
            await _context.SaveChangesAsync(cancellationToken);

            return new TokensResult(refreshToken.Id, accessToken);
        }
    }
}
