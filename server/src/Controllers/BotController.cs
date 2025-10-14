using FlowerBot.src.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Reflection.Metadata;
using Telegram.Bot.Types;

namespace FlowerBot.src.Controllers
{
    [ApiController]
    [Route("api/bot")]
    public class BotController : ControllerBase
    {
        private readonly ILogger<BotController> _logger;
        private readonly ITelegramUpdateHandler _handler;
        public BotController(ILogger<BotController> logger, 
                             ITelegramUpdateHandler handler)
        {
            _logger = logger;
            _handler = handler;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Update update)
        {
            await _handler.HandleAsync(update);
            return Ok();
        }
    }
}
