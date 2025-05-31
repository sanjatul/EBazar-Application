using EBazar.API.Services.Interfaces;

namespace EBazar.API.Services
{
    public class ImageService : IImageService
    {
        private readonly IWebHostEnvironment _environment;
        private readonly ILogger<ImageService> _logger;

        public ImageService(IWebHostEnvironment environment, ILogger<ImageService> logger)
        {
            _environment = environment;
            _logger = logger;
        }

        public async Task<string> SaveImageAsync(IFormFile imageFile)
        {
            try
            {
                var uploadsFolder = Path.Combine(_environment.WebRootPath, "images", "products");

                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                var uniqueFileName = Guid.NewGuid().ToString() + "_" + imageFile.FileName;
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await imageFile.CopyToAsync(fileStream);
                }

                return "/images/products/" + uniqueFileName;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error saving image file");
                throw new InvalidOperationException("Failed to save image file");
            }
        }

        public void DeleteImage(string imagePath)
        {
            try
            {
                if (string.IsNullOrEmpty(imagePath)) return;

                var fullPath = Path.Combine(_environment.WebRootPath, imagePath.TrimStart('/'));
                if (File.Exists(fullPath))
                {
                    File.Delete(fullPath);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting image file: {ImagePath}", imagePath);
            }
        }
    }
}
