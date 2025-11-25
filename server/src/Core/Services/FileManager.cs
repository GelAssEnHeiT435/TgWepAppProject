using FlowerBot.src.Core.Interfaces;
using FlowerBot.src.Data.Models.Common;
using Telegram.Bot.Types;

namespace FlowerBot.src.Core.Services
{
    public class FileManager: IFileManager
    {
        private readonly IWebHostEnvironment _environment;
        private readonly IConfiguration _configuration;

        public FileManager(IWebHostEnvironment environment,
                           IConfiguration configuration)
        {
            _environment = environment;
            _configuration = configuration;
        }

        public async Task<ImageUploadResult?> SaveImageAsync(IFormFile file, CancellationToken ct = default)
        {
            if (file == null || file.Length == 0 || !IsImage(file))
                return null;

            string directory = _configuration["Paths:ImageStorage"];
            var uploadsDir = Path.Combine(_environment.ContentRootPath, directory);
            Directory.CreateDirectory(uploadsDir);

            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
            var filePath = Path.Combine(uploadsDir, fileName);

            await using var stream = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None, 4096, true);
            await file.CopyToAsync(stream, ct);

            return new ImageUploadResult(fileName, $"/api/images/{fileName}");
        }

        public async Task DeleteImageAsync(string fileName, CancellationToken ct = default)
        {
            if (string.IsNullOrWhiteSpace(fileName)) return;

            string fullPath = Path.Combine(_environment.ContentRootPath, _configuration["Paths:ImageStorage"], fileName);

            if(File.Exists(fullPath)) File.Delete(fullPath);
        }

        private bool IsImage(IFormFile file)
        {
            var allowed = new[] { "image/jpeg", "image/png", "image/jpg" };
            return allowed.Contains(file.ContentType);
        }
    }
}
