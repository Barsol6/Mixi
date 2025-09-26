namespace Mixi.Api.Modules.Pdf;

public class FileStorageService: IFileStorageService
{
    private readonly IWebHostEnvironment _environment;
    private readonly IConfiguration _configuration;
    private readonly ILogger<FileStorageService> _logger;
    private readonly string _storagePath;
    private readonly long _maxFileSize;

    public FileStorageService(IWebHostEnvironment environment,
        IConfiguration configuration,
        ILogger<FileStorageService> logger)
    {
        _environment = environment;
        _configuration = configuration;
        _logger = logger;
        
        _storagePath = Path.Combine(_environment.ContentRootPath, "Uploads","Pdfs");
        _maxFileSize = _configuration.GetValue<long>("FileStorage:DatabaseSizeThresholdBytes", 1024 * 1024 * 20);;
        
        if (!Directory.Exists(_storagePath))
        {
            Directory.CreateDirectory(_storagePath);
        }
    }
    
    public StorageStrategy DetermineStorageStrategy(long fileSize)
    {
        return fileSize <= _maxFileSize 
            ? StorageStrategy.Database
            : StorageStrategy.File;
    }
    
    public async Task<string> SaveFileAsync(byte[] fileContent, string? filePath)
    {
        try
        {
            string? safeFileName = Path.GetFileNameWithoutExtension(filePath)?
                .Replace(" ", "_")
                .Replace(",", "")
                .Replace(".", "");
            
            var uniqueFileName = $"{Guid.NewGuid()}_{safeFileName}.pdf";
            var fullPath = Path.Combine(_storagePath, uniqueFileName);
            
            await File.WriteAllBytesAsync(fullPath, fileContent);
            
            _logger.LogInformation($"File {uniqueFileName} saved to {fullPath}");
            
            return uniqueFileName;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error while saving file");
            throw;
        }
    }
    
    public async Task<byte[]?> GetFileAsync(string filePath)
    {
        
        try
        {
            var fullPath = Path.Combine(_storagePath, filePath);
        
            _logger.LogInformation("Looking for file at: {FullPath}", fullPath);
            
            if (!File.Exists(fullPath))
            {
                _logger.LogWarning($"File {filePath} not found at {fullPath}");
                return null;
            }

            return await File.ReadAllBytesAsync(fullPath);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error while getting file");
            throw;
        }
    }
    
    public async Task DeleteFileAsync(string filePath)
    {
        var fullPath = Path.Combine(_storagePath, filePath);
        try
        {
            if (!File.Exists(filePath))
            {
                File.Delete(filePath);
                await Task.CompletedTask;
                
                _logger.LogInformation($"File {filePath} deleted");
            }
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error while deleting file");
            throw;
        }
    }
}