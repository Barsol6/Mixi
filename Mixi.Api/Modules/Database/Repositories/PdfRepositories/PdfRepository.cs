using Microsoft.EntityFrameworkCore;
using Mixi.Api.Modules.Pdf;
using Mixi.Api.Modules.Users;

namespace Mixi.Api.Modules.Database.Repositories.PdfRepositories;

public class PdfRepository:IPdfRepository
{
    private readonly MixiDbContext _dbContext;
    private readonly ILogger<PdfRepository> _logger;
    private readonly IFileStorageService _fileStorageService;
    
    public PdfRepository(MixiDbContext dbContext,
        ILogger<PdfRepository> logger,
        IFileStorageService fileStorageService)
    {
        _dbContext = dbContext;
        _logger = logger;
        _fileStorageService = fileStorageService;
    }
    
    public async Task<PdfDocument?> GetByIdAsync(int id)
    {
        try
        {
            var document = await _dbContext.PdfDocuments.FindAsync(id);
            return document;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error while getting pdf document");
            throw;
        }
    }

    public async Task<List<PdfDocument>> GetAllAsync(string id)
    {
        try
        {
            return await _dbContext.PdfDocuments.Where(x => x.UserName == id).ToListAsync();
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error while getting all pdf documents");
            throw;
        }
    }
    
    public async Task<int> SaveAsync(PdfDocument pdfDocument, byte[] fileContent)
    {
        if (fileContent == null || fileContent.Length == 0)
        {
            throw new ArgumentException("File content cannot be null or empty");
        }
        using var transaction = await _dbContext.Database.BeginTransactionAsync();

        try
        {
            pdfDocument.StorageStrategy = _fileStorageService.DetermineStorageStrategy(fileContent.Length);
            

            if (pdfDocument.StorageStrategy == StorageStrategy.Database)
            {
                pdfDocument.Content = fileContent;
                pdfDocument.FilePath = null;
            }
            else
            {
                pdfDocument.FilePath = await _fileStorageService.SaveFileAsync(fileContent, pdfDocument.Name);
                pdfDocument.Content = Array.Empty<byte>();
            }

            if (pdfDocument.Id != 0)
            {
                var existingPdfDocument = await _dbContext.PdfDocuments.FindAsync(pdfDocument.Id);
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

                existingPdfDocument.Name = pdfDocument.Name;
                existingPdfDocument.Content = pdfDocument.Content;
                existingPdfDocument.FilePath = pdfDocument.FilePath;
                existingPdfDocument.StorageStrategy = pdfDocument.StorageStrategy;
                existingPdfDocument.FormData = pdfDocument.FormData;
                existingPdfDocument.UpdatedAt = DateTime.UtcNow;
                existingPdfDocument.UserName = pdfDocument.UserName;

                _dbContext.Update(existingPdfDocument);

                pdfDocument = existingPdfDocument;

            }
            else
            {
                pdfDocument.CreatedAt = DateTime.UtcNow;
                pdfDocument.UpdatedAt = DateTime.UtcNow;

                if (string.IsNullOrEmpty(pdfDocument.FormData))
                {
                    pdfDocument.FormData = "{}";
                }

                _dbContext.Add(pdfDocument);
            }

            await _dbContext.SaveChangesAsync();
            await transaction.CommitAsync();

            return pdfDocument.Id;
        }
        catch (Exception e)
        {
            await transaction.RollbackAsync();
            
            if (pdfDocument.StorageStrategy == StorageStrategy.File &&
                !string.IsNullOrEmpty(pdfDocument.FilePath))
            {
                await _fileStorageService.DeleteFileAsync(pdfDocument.FilePath);
            }
            _logger.LogError(e, "Error while saving pdf document");
            throw;
        }
    }
    
    public async Task<byte[]?> GetFileContentAsync(int id)
    {
        try
        {
            var document = await _dbContext.PdfDocuments.FindAsync(id);
            if (document is null)
            {
                _logger.LogError($"PdfDocument with id {id} not found");
                return null;
            }

            if (document.StorageStrategy == StorageStrategy.Database)
            {
                return document.Content;
            }
            else if (document.StorageStrategy == StorageStrategy.File &&
                     !string.IsNullOrEmpty(document.FilePath))
            {
                return await _fileStorageService.GetFileAsync(document.FilePath);
            }
            var content = await _fileStorageService.GetFileAsync(document.FilePath);
            if (content == null)
            {
                _logger.LogWarning("File content not found for document {Id} at path: {FilePath}", 
                    id, document.FilePath);
                if (document.Content != null && document.Content.Length > 0)
                {
                    _logger.LogInformation("Using database content as fallback for document {Id}", id);
                    return document.Content;
                }
            }

            return content;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error while getting file content");
            throw;
        }
    }

    public async Task<bool> UpdateFormDatasAsync(int id, string formData)
    {
        using var transaction = await _dbContext.Database.BeginTransactionAsync();

        try
        {
            var pdfDocument = await _dbContext.PdfDocuments.FindAsync(id);
            
            if (pdfDocument is null)
            {
                _logger.LogError($"PdfDocument with id {id} not found");
                return false;
            }
            
            pdfDocument.FormData = formData;
            pdfDocument.UpdatedAt = DateTime.UtcNow;
            
            await _dbContext.SaveChangesAsync();
            await transaction.CommitAsync();
            
            _logger.LogInformation($"FormData for PdfDocument with id {id} updated");
            return true;
        }
        catch (Exception e)
        {
            await transaction.RollbackAsync();
            _logger.LogError(e, "Error while updating form data");
            throw;
        }
    }
    
    public async Task<bool> DeleteAsync(int id)
    {
        using var transaction = await _dbContext.Database.BeginTransactionAsync();

        try
        {
            var document = await _dbContext.PdfDocuments.FindAsync(id);
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
            
            _dbContext.PdfDocuments.Remove(document);
            await _dbContext.SaveChangesAsync();
            await transaction.CommitAsync();
            
            _logger.LogInformation($"PdfDocument with id {id} deleted");
            return true;
        }
        catch (Exception e)
        {
            await transaction.RollbackAsync();
            _logger.LogError(e, "Error while deleting pdf document");
            throw;
        }
    }


    public async Task<List<PdfDocument>> GetRecentDocumentsAsync(int count)
    {
        try
        {
           return await _dbContext.PdfDocuments
               .OrderByDescending(x => x.CreatedAt)
                .Take(count)
                .ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Błąd podczas pobierania ostatnich {Count} dokumentów", count);
            throw;
        }
    }

}