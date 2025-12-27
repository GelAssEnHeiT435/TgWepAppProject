using FlowerBot.src.Core.Interfaces;
using System.Linq;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace FlowerBot.src.Core.Telegram
{
    public class TelegramUpdateHandler: ITelegramUpdateHandler
    {
        private readonly ITelegramBotClient _tgbot;
        private readonly HashSet<long> _adminIds;
        public TelegramUpdateHandler(IServiceProvider provider, IConfiguration configuration)
        {
            _tgbot = provider.GetRequiredService<ITelegramBotClient>();
            _adminIds = configuration
                .GetSection("TelegramBot:Admins")
                .Get<List<long>>()
                ?.ToHashSet() ?? new HashSet<long>();
        }

        public async Task HandleAsync(Update update)
        {
            var chatId = update?.Message?.Chat.Id;

            if (update.Message.Text.ToLower() == "/start")
            {
                List<KeyboardButton> buttons = new List<KeyboardButton>();
                {
                    new KeyboardButton("Открыть веб страницу"){ WebApp = new WebAppInfo{
                        Url = "https://ivy-web.ru"
                    }};
                }

                if (_adminIds.Contains(update.Message.From!.Id))
                {
                    buttons.Add(new KeyboardButton("Открыть тестовую страницу"){ WebApp = new WebAppInfo{
                        Url = "https://t.me/IvyPykhtyolkinBot/market"
                    }});
                }

                await _tgbot.SendMessage(
                    chatId: chatId,
                    text: "Нажмите кнопку для открытия сайта:",
                    replyMarkup: new ReplyKeyboardMarkup(new[] {
                        buttons
                    })
                    {
                        ResizeKeyboard = true
                    });
            }
        }
    }
}
