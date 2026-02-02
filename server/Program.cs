using FlowerBot.Extensions;
using FlowerBot.src.Core.Interfaces;
using FlowerBot.src.Core.Services;
using FlowerBot.src.Core.Telegram;
using FlowerBot.src.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Net;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;

namespace FlowerBot
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.AddApplicationServices();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            await app.UseApplicationMiddleware();

            app.Run();
        }
    }
}