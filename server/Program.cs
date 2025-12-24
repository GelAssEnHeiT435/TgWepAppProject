using FlowerBot.src.Core.Interfaces;
using FlowerBot.src.Core.Services;
using FlowerBot.src.Core.Telegram;
using FlowerBot.src.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;

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
                    policy.WithOrigins(builder.Configuration["AllowedOrigins:Development"]!)
                        .AllowCredentials()
                        .AllowAnyMethod()
                        .AllowAnyHeader();
                });

                options.AddPolicy("ProductionCors", policy =>
                {
                    policy.WithOrigins(builder.Configuration["AllowedOrigins:Production"]!)
                        .AllowCredentials()
                        .AllowAnyMethod()
                        .AllowAnyHeader();
                });
            });

            builder.Services.AddAuthorization();
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(opt =>
                {
                    opt.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidIssuer = AuthOptions.ISSUER,
                        ValidateAudience = true,
                        ValidAudience = AuthOptions.AUDIENCE,
                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.Zero,
                        IssuerSigningKey = AuthOptions.GetSymmetricSecurityKey(),
                        ValidateIssuerSigningKey = true,
                    };
                });
            
            // Add services to the container.
            builder.Services.AddSingleton<ITelegramBotClient>(sp =>
                    new TelegramBotClient(builder.Configuration["TelegramBot:Token"]!));
            builder.Services.AddSingleton<ITelegramUpdateHandler, TelegramUpdateHandler>();

            builder.Services.AddDbContext<ApplicationContext>(options =>
                options.UseNpgsql(builder.Configuration["Connections:Postgres"]));

            builder.Services.AddSingleton<IFileManager, FileManager>();
            builder.Services.AddSingleton<IAuthService, AuthService>();

            builder.Services.AddControllers();

            // Add Swagger service
            builder.Services.AddSwaggerGen(doc =>
            {
                doc.SwaggerDoc("test", new OpenApiInfo
                {
                    Title = "TgWebApp API Doc",
                    Version = "v1"
                });

                doc.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Введите 'Bearer {токен}' (без кавычек)"
                });

                doc.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[] {}
                    }
                });
            });

            builder.Services.ConfigureTelegramBot<Microsoft.AspNetCore.Mvc.JsonOptions>(opt => opt.JsonSerializerOptions);
            builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            // Статика
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(
                    Path.Combine(builder.Environment.ContentRootPath, builder.Configuration["Paths:ImageStorage"]!)),
                RequestPath = "/api/images"
            });

            app.UseRouting();

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
                db.Database.Migrate();
            }

            app.UseHttpsRedirection();
            app.UseAuthentication();
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
