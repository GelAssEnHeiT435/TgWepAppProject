using FlowerBot.src.Core.Interfaces;
using FlowerBot.src.Data;
using FlowerBot.src.Data.Models.Common;
using FlowerBot.src.Data.Models.Database;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FlowerBot.src.Core.Handlers.Authorization
{
    public record class RefreshUserSessionCommand(Guid? cookieToken): IRequest<string?>;

    public class RefreshUserSessionHandler : IRequestHandler<RefreshUserSessionCommand, string?>
    {
        private readonly ApplicationContext _context;
        private readonly IAuthService _authService;

        public RefreshUserSessionHandler(ApplicationContext context, IAuthService authService)
        {
            _context = context;
            _authService = authService;
        }

        public async Task<string?> Handle(RefreshUserSessionCommand request, CancellationToken cancellationToken)
        {
            RefreshToken? token = _context.RefreshTokens.FirstOrDefault(rt => rt.Id == request.cookieToken);

            if (token == null || token.IsRevoked || token.ExpiresAt < DateTime.UtcNow) return null;

            string accessToken = _authService.GenerateJwt(token.TelegramId, TimeSpan.FromMinutes(15));
            return accessToken;
        }
    }
}
