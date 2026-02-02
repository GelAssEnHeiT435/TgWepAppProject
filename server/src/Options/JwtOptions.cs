using System.ComponentModel.DataAnnotations;

namespace FlowerBot.src.Options
{
    public class JwtOptions
    {
        [Required] public string Issuer { get; init; }

        [Required] public string Audience { get; init; }

        [Required]
        [StringLength(128, MinimumLength = 32)]
        public string Key { get; init; }
    }
}
