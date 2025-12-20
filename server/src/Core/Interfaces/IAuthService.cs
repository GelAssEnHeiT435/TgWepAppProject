namespace FlowerBot.src.Core.Interfaces
{
    public interface IAuthService
    {
        string GenerateJwt(long telegramId, TimeSpan expires);
    }
}
