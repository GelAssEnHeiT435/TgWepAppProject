using System.ComponentModel.DataAnnotations;

namespace FlowerBot.src.Options
{
    public class PathsOptions
    {
        [Required] public string ImageStorage { get; init; }
    }
}
