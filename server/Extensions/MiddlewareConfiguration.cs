using FlowerBot.src.Data;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using System.Net;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;

namespace FlowerBot.Extensions
{
    public static class MiddlewareConfiguration
    {
        public static async Task UseApplicationMiddleware(this WebApplication app)
        {
            app.UseForwardedHeaders();
            app.UseRouting();
            app.UseStaticFiles();
            await app.UseEnviromentServices();
            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllers();
            await app.SetupTelegramWebhookAsync();
        }

        /// <summary>
        /// Configuring headers for Nginx proxy
        /// </summary>
        private static void UseForwardedHeaders(this WebApplication app)
        {
            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor |
                       ForwardedHeaders.XForwardedProto |
                       ForwardedHeaders.XForwardedHost,
                KnownNetworks = { new Microsoft.AspNetCore.HttpOverrides.IPNetwork(IPAddress.Parse("127.0.0.1"), 32) },
                KnownProxies = { IPAddress.Parse("127.0.0.1") }
            });
        }

        /// <summary>
        /// Add static files for getting images
        /// </summary>
        private static void UseStaticFiles(this WebApplication app)
        {
            var builder = WebApplication.CreateBuilder(Array.Empty<string>());
            var imageStoragePath = app.Configuration["Paths:ImageStorage"]!;

            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(
                    Path.Combine(app.Environment.ContentRootPath, imageStoragePath)),
                RequestPath = "/api/images"
            });
        }

        /// <summary>
        /// Enviroment's settings
        /// </summary>
        private static async Task UseEnviromentServices(this WebApplication app)
        {
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(option => {
                    option.SwaggerEndpoint("/swagger/test/swagger.json", "TgWebApp API v1");
                });
                app.UseCors("DevelopmentCors");
            }
            else
            {
                app.UseCors("ProductionCors");
            }

            if (app.Environment.IsStaging())
            {
                using var scope = app.Services.CreateScope();
                var db = scope.ServiceProvider.GetRequiredService<ApplicationContext>();
                await db.Database.MigrateAsync();
            }
        }

        /// <summary>
        /// Tg webhook for bot
        /// </summary>
        private static async Task SetupTelegramWebhookAsync(this WebApplication app)
        {
            var bot = app.Services.GetRequiredService<ITelegramBotClient>();
            var webhookUrl = app.Configuration["TelegramBot:WebhookUrl"]!;

            await bot.SetWebhook(
                url: $"{webhookUrl}/api/bot",
                allowedUpdates: new[]
                {
                    Telegram.Bot.Types.Enums.UpdateType.Message,
                    Telegram.Bot.Types.Enums.UpdateType.CallbackQuery
                },
                cancellationToken: CancellationToken.None);
        }
    }
}
