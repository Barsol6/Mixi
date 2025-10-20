using Mixi.Api.Modules.Pdf;
using Mixi.Api.Modules.Users;
using MongoDB.Bson;

namespace Mixi.Api.Modules.Database.Repositories.PdfRepositories;

public interface IPdfRepository
{
    Task<PdfDocument?> GetByIdAsync(string id);
    Task<List<PdfDocument>> GetAllAsync(string id);
    Task<byte[]?> GetFileContentAsync(string id);
    Task<string> SaveAsync(PdfDocument pdfDocument,byte[] content);
    Task<bool> UpdateFormDatasAsync(string id, string formData);
    Task<bool> DeleteAsync(string id);
    Task<List<PdfDocument>> GetRecentDocumentsAsync(int count);
}