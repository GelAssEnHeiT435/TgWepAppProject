using FlowerBot.src.Core.Interfaces;
using FlowerBot.src.Data;
using FlowerBot.src.Data.Models.Database;
using MediatR;

namespace FlowerBot.src.Core.Handlers.Authorization
{
    public record class LogoutUserCommand(Guid cookieToken): IRequest;

    public class LogoutUserHandler : IRequestHandler<LogoutUserCommand>
    {
        private readonly ApplicationContext _context;

        public LogoutUserHandler(ApplicationContext context)
        {
            _context = context;
        }

        public async Task Handle(LogoutUserCommand request, CancellationToken cancellationToken)
        {
            RefreshToken token = _context.RefreshTokens.FirstOrDefault(rt => rt.Id == request.cookieToken)!;
            if (token != null)
            {
                token.IsRevoked = true;
                token.RevokedReason = "User logged out";
                await _context.SaveChangesAsync(cancellationToken);
            }
        }
    }
}
