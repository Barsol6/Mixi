using Mixi.Api.Modules.Pdf;

namespace Mixi.Api.Modules.Database.Repositories.PdfRepositories;

public interface IPdfRepository
{
    Task<PdfDocument?> GetByIdAsync(string id, string userName);
    Task<List<PdfDocument>> GetAllAsync(string id);
    Task<byte[]?> GetFileContentAsync(string id, string userName);
    Task<string> SaveAsync(PdfDocument pdfDocument, byte[] content);
    Task<bool> UpdateFormDatasAsync(string id, string formData, string userName);
    Task<bool> DeleteAsync(string id, string userName);
    Task<List<PdfDocument>> GetRecentDocumentsAsync(int count);
}