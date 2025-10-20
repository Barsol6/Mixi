using System.Text.RegularExpressions;
using Mixi.Api.Modules.Pdf;
using MongoDB.Bson;
using MongoDB.Driver;


namespace Mixi.Api.Modules.Database.Repositories.PdfRepositories;

public class PdfRepository:IPdfRepository
{
    private readonly MongoMixiDbContext _mongoDbContext;
    private readonly ILogger<PdfRepository> _logger;
    private readonly IFileStorageService _fileStorageService;
    
    public PdfRepository(MongoMixiDbContext dbContext,
        ILogger<PdfRepository> logger,
        IFileStorageService fileStorageService)
    {
        _mongoDbContext = dbContext;
        _logger = logger;
        _fileStorageService = fileStorageService;
    }
    
    public async Task<PdfDocument?> GetByIdAsync(string id)
    {
        try
        {
            return await _mongoDbContext.PdfDocuments.Find(p => p.Id == id).FirstOrDefaultAsync();
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error while getting pdf document");
            throw;
        }
    }

    public async Task<List<PdfDocument>> GetAllAsync(string userName)
    {
        try
        {
            var filter = Builders<PdfDocument>.Filter.Regex(
                x => x.UserName,
                new MongoDB.Bson.BsonRegularExpression($"^{Regex.Escape(userName)}$", "i")
            );
            return await _mongoDbContext.PdfDocuments.Find(filter).ToListAsync();
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error while getting all pdf documents");
            throw;
        }
    }
    
    public async Task<string> SaveAsync(PdfDocument pdfDocument, byte[] fileContent)
    {
        if (fileContent == null || fileContent.Length == 0)
        {
            throw new ArgumentException("File content cannot be null or empty");
        }
    

        try
        {
            pdfDocument.StorageStrategy = _fileStorageService.DetermineStorageStrategy(fileContent.Length);
            

            if (pdfDocument.StorageStrategy == StorageStrategy.Database)
            {
                pdfDocument.FileContent = fileContent;
                pdfDocument.FilePath = null;
            }
            else
            {
                pdfDocument.FilePath = await _fileStorageService.SaveFileAsync(fileContent, pdfDocument.FileName);
                pdfDocument.FileContent = Array.Empty<byte>();
            }

            var isNew = pdfDocument.Id == string.Empty;
            
            if (!isNew)
            {
                var existingPdfDocument = await _mongoDbContext.PdfDocuments.Find(x => x.Id == pdfDocument.Id).FirstOrDefaultAsync();
               
                if (existingPdfDocument is null)
                {
                    throw new KeyNotFoundException($"PdfDocument with id {pdfDocument.Id} not found");
                }

                if (existingPdfDocument.StorageStrategy == StorageStrategy.File &&
                    !string.IsNullOrEmpty(existingPdfDocument.FilePath) &&
                    (pdfDocument.StorageStrategy == StorageStrategy.Database||
                     pdfDocument.FilePath != existingPdfDocument.FilePath))
                {
                    await _fileStorageService.DeleteFileAsync(existingPdfDocument.FilePath);
                }

                existingPdfDocument.FileName = pdfDocument.FileName;
                existingPdfDocument.FileContent = pdfDocument.FileContent;
                existingPdfDocument.FilePath = pdfDocument.FilePath;
                existingPdfDocument.StorageStrategy = pdfDocument.StorageStrategy;
                existingPdfDocument.FormData = pdfDocument.FormData;
                existingPdfDocument.UpdatedAt = DateTime.UtcNow;
                existingPdfDocument.UserName = pdfDocument.UserName;
            }
            else
            {
                pdfDocument.CreatedAt = DateTime.UtcNow;
                pdfDocument.UpdatedAt = DateTime.UtcNow;

                if (string.IsNullOrEmpty(pdfDocument.FormData))
                {
                    pdfDocument.FormData = "{}";
                }
                
                await _mongoDbContext.PdfDocuments.InsertOneAsync(pdfDocument);
            }
            

            return pdfDocument.Id.ToString();;
        }
        catch (Exception e)
        {
            
            if (pdfDocument.StorageStrategy == StorageStrategy.File &&
                !string.IsNullOrEmpty(pdfDocument.FilePath))
            {
                await _fileStorageService.DeleteFileAsync(pdfDocument.FilePath);
            }
            _logger.LogError(e, "Error while saving pdf document");
            throw;
        }
    }
    
    public async Task<byte[]?> GetFileContentAsync(string id)
    {
        try
        {
            var document = await GetByIdAsync(id);
            if (document is null)
            {
                _logger.LogError($"PdfDocument with id {id} not found");
                return null;
            }

            if (document.StorageStrategy == StorageStrategy.Database)
            {
                return document.FileContent;
            }
            else if (document.StorageStrategy == StorageStrategy.File &&
                     !string.IsNullOrEmpty(document.FilePath))
            {
                return await _fileStorageService.GetFileAsync(document.FilePath);
            }

            _logger.LogError($"File content for PdfDocument with id {id} not found");
            return null;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error while getting file content");
            throw;
        }
    }

    public async Task<bool> UpdateFormDatasAsync(string id, string formData)
    {
        try
        {
            
            
            var pdfDocument = await _mongoDbContext.PdfDocuments.Find(x => x.Id == id).FirstOrDefaultAsync();
            
            if (pdfDocument is null)
            {
                _logger.LogError($"PdfDocument with id {id} not found");
                return false;
            }
            
            pdfDocument.FormData = formData;
            pdfDocument.UpdatedAt = DateTime.UtcNow;
            
            await _mongoDbContext.PdfDocuments.ReplaceOneAsync(x => x.Id == id, pdfDocument);
            
            _logger.LogInformation($"FormData for PdfDocument with id {id} updated");
            return true;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error while updating form data");
            throw;
        }
    }
    
    public async Task<bool> DeleteAsync(string id)
    {
       
        try
        {
            var document = await _mongoDbContext.PdfDocuments.Find(x => x.Id == id).FirstOrDefaultAsync();
            if (document is null)
            {
                _logger.LogError($"PdfDocument with id {id} not found");
                return false;
            }

            if (document.StorageStrategy == StorageStrategy.File &&
                !string.IsNullOrEmpty(document.FilePath))
            {
                await _fileStorageService.DeleteFileAsync(document.FilePath);
                _logger.LogInformation($"File {document.FilePath} deleted");
            }
            
            await _mongoDbContext.PdfDocuments.DeleteOneAsync(x => x.Id == document.Id);
     
            _logger.LogInformation($"PdfDocument with id {id} deleted");
            return true;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error while deleting pdf document");
            throw;
        }
    }


    public async Task<List<PdfDocument>> GetRecentDocumentsAsync(int count)
    {
        try
        {
           return await _mongoDbContext.PdfDocuments
               .Find(_ => true )
               .SortByDescending(x => x.CreatedAt)
               .Limit(count)
               .ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Błąd podczas pobierania ostatnich {Count} dokumentów", count);
            throw;
        }
    }

}