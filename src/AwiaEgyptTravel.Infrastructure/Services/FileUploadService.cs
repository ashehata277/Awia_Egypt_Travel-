using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

namespace AwiaEgyptTravel.Infrastructure.Services
{
    public interface IFileUploadService
    {
        Task<string> UploadImageAsync(IFormFile file, string folderPath, int maxWidth = 800);
        Task DeleteImageAsync(string filePath);
    }

    public class FileUploadService : IFileUploadService
    {
        private readonly IWebHostEnvironment _environment;

        public FileUploadService(IWebHostEnvironment environment)
        {
            _environment = environment;
        }

        public async Task<string> UploadImageAsync(IFormFile file, string folderPath, int maxWidth = 800)
        {
            if (file == null || file.Length == 0)
                throw new ArgumentException("No file was provided");

            var uploadsFolder = Path.Combine(_environment.WebRootPath, folderPath);
            Directory.CreateDirectory(uploadsFolder);

            var uniqueFileName = $"{Guid.NewGuid()}_{Path.GetFileName(file.FileName)}";
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);

            using (var image = await Image.LoadAsync(file.OpenReadStream()))
            {
                if (image.Width > maxWidth)
                {
                    var scaleFactor = (double)maxWidth / image.Width;
                    var newHeight = (int)(image.Height * scaleFactor);
                    image.Mutate(x => x.Resize(maxWidth, newHeight));
                }

                await image.SaveAsync(filePath);
            }

            return $"/{folderPath}/{uniqueFileName}";
        }

        public Task DeleteImageAsync(string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
                return Task.CompletedTask;

            var fullPath = Path.Combine(_environment.WebRootPath, filePath.TrimStart('/'));
            if (File.Exists(fullPath))
            {
                File.Delete(fullPath);
            }

            return Task.CompletedTask;
        }
    }
}
