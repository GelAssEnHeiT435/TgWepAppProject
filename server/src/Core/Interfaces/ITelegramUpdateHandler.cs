using Telegram.Bot.Types;

namespace FlowerBot.src.Core.Interfaces
{
    public interface ITelegramUpdateHandler
    {
        Task HandleAsync(Update update);
    }
}
