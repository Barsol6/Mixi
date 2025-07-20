namespace mixi.Modules.Pdf;

public interface IFileStorageService
{
    Task<string> SaveFileAsync(byte[] fileContent, string? filePath);
    Task<byte[]?> GetFileAsync(string filePath);
    Task DeleteFileAsync(string filePath);
    StorageStrategy DetermineStorageStrategy(long fileSize);
}