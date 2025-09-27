using System.ComponentModel.DataAnnotations;

namespace Mixi.Api.Modules.Pdf;

public class PdfDocument
{
    
    [Key]
    public int Id { get; set; }
    
    [Required]
    [MaxLength(255)]
    public string? Name { get; set; }
    
    [Required]
    public byte[] Content { get; set; }
    
    [MaxLength(512)]
    public string? FilePath { get; set; }
    
    public StorageStrategy StorageStrategy { get; set; }
    
    public string? FormData { get; set; }
    
    public DateTime CreatedAt { get; set; } 
    
    public DateTime UpdatedAt { get; set; }

    

   
}
public enum StorageStrategy
{
    Database,
    File
}