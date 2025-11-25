using FlowerBot.src.Core.Interfaces;
using FlowerBot.src.Core.Services;
using FlowerBot.src.Core.Telegram;
using FlowerBot.src.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.OpenApi.Models;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;
using TL;

namespace FlowerBot
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // add CORS Policy
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("DevelopmentCors", policy =>
                {
                    policy.WithOrigins(builder.Configuration["AllowedOrigins:Development"])
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials();
                });

                options.AddPolicy("ProductionCors", policy =>
                {
                    policy.WithOrigins(builder.Configuration["AllowedOrigins:Production"])
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials();
                });
            });
            
            // Add services to the container.
            builder.Services.AddSingleton<ITelegramBotClient>(sp =>
                    new TelegramBotClient(builder.Configuration["TelegramBot:Token"]));
            builder.Services.AddSingleton<ITelegramUpdateHandler, TelegramUpdateHandler>();

            builder.Services.AddDbContext<ApplicationContext>(options =>
                options.UseNpgsql(builder.Configuration["Connections:Postgres"]));

            builder.Services.AddSingleton<IFileManager, FileManager>();

            builder.Services.AddControllers();

            // Add Swagger service
            builder.Services.AddSwaggerGen(doc =>
            {
                doc.SwaggerDoc("test", new OpenApiInfo
                {
                    Title = "TgWebApp API Doc",
                    Version = "v1"
                });
            });

            builder.Services.ConfigureTelegramBot<Microsoft.AspNetCore.Mvc.JsonOptions>(opt => opt.JsonSerializerOptions);
            builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));

            var app = builder.Build();

            // Configure the HTTP request pipeline.

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(option => {
                    option.SwaggerEndpoint(url: "/swagger/test/swagger.json", 
                                           name: "DirectoryService API v1");
                });
                app.UseCors("DevelopmentCors");
            }

            if (app.Environment.IsProduction())
            {
                app.UseCors("ProductionCors");
            }

            // uploading photos via the link
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(
                    Path.Combine(builder.Environment.ContentRootPath, builder.Configuration["Paths:ImageStorage"])),
                RequestPath = "/api/images"
            });

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
