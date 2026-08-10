namespace Services.FileStorage
{
    public interface IFileStorageService
    {
        Task<string> SaveAsync(string container, string fileName, Stream content);
        Task<Stream?> GetAsync(string container, string fileName);
        Task<bool> DeleteAsync(string container, string fileName);
        Task<string?> GetUrl(string relativePath);
    }
}
