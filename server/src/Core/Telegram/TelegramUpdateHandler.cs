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
            var fromId = update?.Message?.From?.Id;

            if (update?.Message?.Text?.ToLower() == "/start")
            {
                var rows = new List<InlineKeyboardButton[]>();

                rows.Add(new[] {
                    InlineKeyboardButton.WithWebApp("Открыть веб страницу",
                        new WebAppInfo { Url = "https://ivy-web.ru" })
                });

                //if (_adminIds.Contains(fromId.Value))
                //{
                //    rows.Add(new[] {
                //        InlineKeyboardButton.WithWebApp("Открыть тестовую страницу",
                //            new WebAppInfo { Url = "https://staging.ivy-web.ru" })
                //    });
                //}

                await _tgbot.SendMessage(
                    chatId: chatId.Value,
                    text: "Нажмите кнопку:",
                    replyMarkup: new InlineKeyboardMarkup(rows)
                );
            }
        }
    }
}
