using System.ComponentModel.DataAnnotations;
using Mixi.Api.Modules.Users;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Mixi.Api.Modules.Pdf;

public class PdfDocument
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = string.Empty;
    public string FileName { get; set; } 
    public byte[]? FileContent { get; set; }
    public string? FilePath { get; set; }
    public DateTime CreatedAt { get; set; }
    public StorageStrategy StorageStrategy { get; set; }
    public string? FormData { get; set; }
    public DateTime UpdatedAt { get; set; }
    public string? UserName { get; set; }
}
public enum StorageStrategy
{
    Database,
    File
}