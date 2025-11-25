using FlowerBot.src.Data.Models.Common;

namespace FlowerBot.src.Core.Interfaces
{
    public interface IFileManager
    {
        Task<ImageUploadResult> SaveImageAsync(IFormFile file, CancellationToken ct = default);
        Task DeleteImageAsync(string fileName, CancellationToken ct = default);
    }
}
