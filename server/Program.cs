using FlowerBot.src.Core.Interfaces;
using FlowerBot.src.Core.Telegram;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;

namespace FlowerBot
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddSingleton<ITelegramBotClient>(sp =>
                    new TelegramBotClient(builder.Configuration["TelegramBot:Token"]));
            builder.Services.AddSingleton<ITelegramUpdateHandler, TelegramUpdateHandler>();
            // Add services to the container.

            builder.Services.AddControllers();
            builder.Services.ConfigureTelegramBot<Microsoft.AspNetCore.Mvc.JsonOptions>(opt => opt.JsonSerializerOptions);

            var app = builder.Build();

            // Configure the HTTP request pipeline.

            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers();

            var bot = app.Services.GetRequiredService<ITelegramBotClient>();
            await bot.SetWebhook(
                url: $"{builder.Configuration["TelegramBot:WebhookUrl"]}/api/bot",
                allowedUpdates: new[] {
                    UpdateType.Message,
                    UpdateType.CallbackQuery
                },
                cancellationToken: CancellationToken.None);

            app.Run();
        }
    }
}
