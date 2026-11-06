namespace MYAcademey_Portfollo.Services
{
    // Görsel yükleme işlemlerini tek bir yerde toplar.
    // Proje, Ana Sayfa ve Hakkımda ekranları aynı kodu tekrar yazmadan bu sınıfı kullanır.
    public class ImageUploadService
    {
        private static readonly string[] AllowedExtensions = { ".jpg", ".jpeg", ".png", ".webp" };
        private const long MaxFileSize = 2 * 1024 * 1024; // 2 MB

        private readonly IWebHostEnvironment _environment;

        public ImageUploadService(IWebHostEnvironment environment)
        {
            _environment = environment;
        }

        // Dosyayı doğrular ve wwwroot/uploads/{folder} altına kaydeder.
        // Başarılıysa veritabanına yazılacak yolu döner (ör: /uploads/projects/abc.jpg).
        public async Task<ImageUploadResult> SaveAsync(IFormFile file, string folder)
        {
            if (file == null || file.Length == 0)
            {
                return ImageUploadResult.Fail("Lütfen bir görsel seçiniz.");
            }

            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
            if (!AllowedExtensions.Contains(extension))
            {
                return ImageUploadResult.Fail("Sadece .jpg, .jpeg, .png veya .webp dosyaları yüklenebilir.");
            }

            if (file.Length > MaxFileSize)
            {
                return ImageUploadResult.Fail("Görsel boyutu en fazla 2 MB olabilir.");
            }

            // Kullanıcının verdiği dosya adı kullanılmaz; çakışma ve güvenlik sorunlarını önlemek için benzersiz ad üretilir.
            var safeFileName = $"{Guid.NewGuid():N}{extension}";
            var uploadFolder = Path.Combine(_environment.WebRootPath, "uploads", folder);
            Directory.CreateDirectory(uploadFolder);

            var fullPath = Path.Combine(uploadFolder, safeFileName);
            using (var stream = new FileStream(fullPath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return ImageUploadResult.Ok($"/uploads/{folder}/{safeFileName}");
        }

        // Sadece bizim yüklediğimiz (/uploads/ altındaki) dosyaları siler; dış bağlantılara dokunmaz.
        public void Delete(string? imagePath)
        {
            if (string.IsNullOrWhiteSpace(imagePath) || !imagePath.StartsWith("/uploads/"))
            {
                return;
            }

            var fullPath = Path.Combine(_environment.WebRootPath, imagePath.TrimStart('/').Replace('/', Path.DirectorySeparatorChar));
            if (File.Exists(fullPath))
            {
                File.Delete(fullPath);
            }
        }
    }

    public class ImageUploadResult
    {
        public bool Success { get; private set; }
        public string? Path { get; private set; }
        public string? ErrorMessage { get; private set; }

        public static ImageUploadResult Ok(string path) => new ImageUploadResult { Success = true, Path = path };
        public static ImageUploadResult Fail(string error) => new ImageUploadResult { Success = false, ErrorMessage = error };
    }
}
