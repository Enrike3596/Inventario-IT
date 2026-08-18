using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace Services.FileStorage
{
    public class FileStorageSettings
    {
        public string BasePath { get; set; } = "uploads";
        public string BaseUrl { get; set; } = "/uploads";
        public long DocumentoRemisionMaxBytes { get; set; } = 10 * 1024 * 1024;
        public string[] DocumentoRemisionExtensiones { get; set; } = { ".pdf" };
    }

    public class FileStorageService : IFileStorageService
    {
        private readonly string _basePath;
        private readonly string _baseUrl;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public FileStorageService(IOptions<FileStorageSettings> settings, IHttpContextAccessor httpContextAccessor)
        {
            _basePath = Path.GetFullPath(settings.Value.BasePath);
            _baseUrl = settings.Value.BaseUrl.TrimEnd('/');
            _httpContextAccessor = httpContextAccessor;
            Directory.CreateDirectory(_basePath);
        }

        public async Task<string> SaveAsync(string container, string fileName, Stream content)
        {
            var dir = Path.Combine(_basePath, container);
            Directory.CreateDirectory(dir);

            var uniqueName = $"{Guid.NewGuid():N}_{fileName}";
            var fullPath = Path.Combine(dir, uniqueName);

            await using var fs = new FileStream(fullPath, FileMode.Create);
            await content.CopyToAsync(fs);

            return Path.Combine(container, uniqueName).Replace('\\', '/');
        }

        public Task<Stream?> GetAsync(string container, string fileName)
        {
            var fullPath = Path.Combine(_basePath, container, fileName);
            if (!File.Exists(fullPath))
                return Task.FromResult<Stream?>(null);

            return Task.FromResult<Stream?>(new FileStream(fullPath, FileMode.Open, FileAccess.Read));
        }

        public Task<bool> DeleteAsync(string container, string fileName)
        {
            var fullPath = Path.Combine(_basePath, container, fileName);
            if (!File.Exists(fullPath))
                return Task.FromResult(false);

            File.Delete(fullPath);
            return Task.FromResult(true);
        }

        public Task<string?> GetUrl(string relativePath)
        {
            if (string.IsNullOrWhiteSpace(relativePath))
                return Task.FromResult<string?>(null);

            var request = _httpContextAccessor.HttpContext?.Request;
            if (request != null)
            {
                var baseUrl = $"{request.Scheme}://{request.Host}";
                return Task.FromResult<string?>($"{baseUrl}{_baseUrl}/{relativePath.Replace('\\', '/')}");
            }

            return Task.FromResult<string?>($"{_baseUrl}/{relativePath.Replace('\\', '/')}");
        }
    }
}
