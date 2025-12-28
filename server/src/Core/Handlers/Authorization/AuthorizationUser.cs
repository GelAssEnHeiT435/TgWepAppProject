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
    public record class AuthorizationUserCommand(string? InitDataRaw) : IRequest<TokensResult?>;

    public class AuthorizationUserHandler : IRequestHandler<AuthorizationUserCommand, TokensResult?>
    {
        private readonly ILogger<AuthorizationUserHandler> _logger;
        private readonly ApplicationContext _context;
        private readonly IAuthService _authService;
        private readonly string _botToken;

        public AuthorizationUserHandler(
            ILogger<AuthorizationUserHandler> logger,
            ApplicationContext context, 
            IAuthService authService,
            IConfiguration configuration)
        {
            _context = context;
            _authService = authService;
            _botToken = configuration["TelegramBot:Token"]
                    ?? throw new InvalidOperationException("Telegram bot token missing");
            _logger = logger;
        }

        public async Task<TokensResult?> Handle(AuthorizationUserCommand request, CancellationToken cancellationToken)
        {
            if (request.InitDataRaw.IsNullOrEmpty())
            {
                _logger.LogWarning("InitDataRaw пустой или null");
                return null;
            }

            if (!InitDataValidator.Validate(request.InitDataRaw!, _botToken))
            {
                _logger.LogWarning("Валидация InitDataRaw не пройдена. Данные: {Data}", request.InitDataRaw);
                return null; 
            }
                

            long? telegramId = InitDataValidator.GetUserId(request.InitDataRaw!);
            if (!telegramId.HasValue || telegramId <= 0)
            {
                _logger.LogWarning("Не удалось извлечь telegramId из InitDataRaw: {Data}", request.InitDataRaw);
                return null;
            }

            _logger.LogInformation("Успешная валидация. Telegram ID: {TelegramId}", telegramId.Value);
            string accessToken = _authService.GenerateJwt(telegramId.Value, TimeSpan.FromMinutes(15));

            RefreshToken refreshToken = new RefreshToken
            {
                Id = Guid.NewGuid(),
                TelegramId = telegramId.Value,
                ExpiresAt = DateTime.UtcNow.AddDays(1)
            };

            _context.RefreshTokens.Add(refreshToken);
            await _context.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Токены успешно созданы для Telegram ID: {TelegramId}", telegramId.Value);
            return new TokensResult(refreshToken.Id, accessToken);
        }
    }
}
