using FlowerBot.src.Core.Interfaces;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace FlowerBot.src.Core.Telegram
{
    public class TelegramUpdateHandler: ITelegramUpdateHandler
    {
        private readonly ITelegramBotClient _tgbot;
        public TelegramUpdateHandler(IServiceProvider provider)
        {
            _tgbot = provider.GetRequiredService<ITelegramBotClient>();
        }

        public async Task HandleAsync(Update update)
        {
            var chatId = update.Message.Chat.Id;

            if (update.Message.Text.ToLower() == "/start")
            {
                // Создаем кнопку веб-приложения
                await _tgbot.SendMessage(
                    chatId: chatId,
                    text: "Нажмите кнопку для открытия сайта:",
                    replyMarkup: new ReplyKeyboardMarkup(new[] {
                        new KeyboardButton("Открыть веб страницу"){ WebApp = new WebAppInfo { 
                            Url = "https://ivy-web.ru" }
                        }
                    })
                    {
                        ResizeKeyboard = true
                    });
            }
        }
    }
}
