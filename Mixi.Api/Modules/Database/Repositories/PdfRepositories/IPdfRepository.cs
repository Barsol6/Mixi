using Mixi.Api.Modules.Pdf;

namespace Mixi.Api.Modules.Database.Repositories.PdfRepositories;

public interface IPdfRepository
{
    Task<PdfDocument?> GetByIdAsync(int id);
    Task<List<PdfDocument>> GetAllAsync();
    Task<byte[]?> GetFileContentAsync(int id);
    Task<int> SaveAsync(PdfDocument pdfDocument,byte[] content);
    Task<bool> UpdateFormDatasAsync(int id, string formData);
    Task<bool> DeleteAsync(int id);
    Task<List<PdfDocument>> GetRecentDocumentsAsync(int count);
}