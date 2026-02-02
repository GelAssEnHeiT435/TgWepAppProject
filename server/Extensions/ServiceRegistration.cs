using FlowerBot.src.Core.Interfaces;
using FlowerBot.src.Core.Services;
using FlowerBot.src.Core.Telegram;
using FlowerBot.src.Data;
using FlowerBot.src.Options;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Telegram.Bot;

namespace FlowerBot.Extensions
{
    public static class ServiceRegistration
    {
        public static void AddApplicationServices(this WebApplicationBuilder builder)
        {
            builder.ConfigureOptions();
            builder.AddCorsPolicy();
            builder.AddAuthenticationServices();
            builder.AddTelegramServices();
            builder.AddDatabase();

            builder.Services.AddControllers();

            builder.AddServices();
            builder.AddSwaggerDoc();
        }

        private static void ConfigureOptions(this WebApplicationBuilder builder) 
        {
            builder.Services.AddOptions<JwtOptions>()
                .BindConfiguration("Jwt")
                .ValidateDataAnnotations()
                .ValidateOnStart();

            builder.Services.AddOptions<TelegramOptions>()
                .BindConfiguration("TelegramBot")
                .ValidateDataAnnotations()
                .ValidateOnStart();
        }

        /// <summary>
        /// add CORS Policy
        /// </summary>
        private static void AddCorsPolicy(this WebApplicationBuilder builder)
        {
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
        }

        /// <summary>
        /// Add Authentification and Authorization
        /// </summary>
        private static void AddAuthenticationServices(this WebApplicationBuilder builder)
        {
            var jwtOptions = builder.Configuration
                .GetSection("Jwt")
                .Get<JwtOptions>();

            builder.Services.AddAuthorization();
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(opt =>
                {
                    opt.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidIssuer = jwtOptions.Issuer,
                        ValidateAudience = true,
                        ValidAudience = jwtOptions.Audience,
                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.Zero,
                        IssuerSigningKey = AuthService.GetSymmetricSecurityKey(jwtOptions.Key),
                        ValidateIssuerSigningKey = true,
                    };
                });
        }

        /// <summary>
        /// Registration of Telegram bot services
        /// </summary>
        private static void AddTelegramServices(this WebApplicationBuilder builder)
        {
            builder.Services.AddSingleton<ITelegramBotClient>(sp =>
                new TelegramBotClient(builder.Configuration["TelegramBot:Token"]!));
            builder.Services.AddSingleton<ITelegramUpdateHandler, TelegramUpdateHandler>();
            builder.Services.ConfigureTelegramBot<Microsoft.AspNetCore.Mvc.JsonOptions>(opt => opt.JsonSerializerOptions);
        }

        /// <summary>
        /// configuring database
        /// </summary>
        private static void AddDatabase(this WebApplicationBuilder builder)
        {
            builder.Services.AddDbContext<ApplicationContext>(options =>
                options.UseNpgsql(builder.Configuration["Connections:Postgres"]));
        }

        /// <summary>
        /// configuring DI container
        /// </summary>
        private static void AddServices(this WebApplicationBuilder builder)
        {
            builder.Services.AddSingleton<IFileManager, FileManager>();
            builder.Services.AddSingleton<IAuthService, AuthService>();
            builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));
        }

        /// <summary>
        /// Add swagger API Documentation
        /// </summary>
        private static void AddSwaggerDoc(this WebApplicationBuilder builder)
        {
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
        }
    }
}
