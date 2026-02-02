using System.ComponentModel.DataAnnotations;

namespace FlowerBot.src.Options
{
    public class TelegramOptions
    {
        [Required] public string Token { get; init; }
        [Required] public string WebhookUrl { get; init; }
        [Required] public List<long> Admins { get; init; }
    }
}
