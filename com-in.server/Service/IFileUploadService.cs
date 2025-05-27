

namespace com_in.server.Service
{
    public interface IFileUploadService
    {
        public Task<(string relativePath, string fullPath)> fileUpload(IFormFile file);
    }

    public class FileUploadService : IFileUploadService
    {
        private readonly IWebHostEnvironment _environment;
        public FileUploadService(IWebHostEnvironment environment) => _environment = environment;


        public async Task<(string relativePath, string fullPath)> fileUpload(IFormFile file)
        {
            
            string? relativePath = "";
            string? fullPath = "";
            var ext = Path.GetExtension(file.FileName).ToLowerInvariant();
            var fileName = $"{Guid.NewGuid()}{ext}";

            relativePath = Path.Combine("Announcements", fileName);
            fullPath = Path.Combine(_environment.ContentRootPath, "uploads", relativePath);

            Directory.CreateDirectory(Path.GetDirectoryName(fullPath));

            using (var stream = new FileStream(fullPath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return (relativePath, fullPath);
        }
    }
}
