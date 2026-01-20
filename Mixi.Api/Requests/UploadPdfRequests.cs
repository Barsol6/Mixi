using System.ComponentModel.DataAnnotations;

namespace Mixi.Api.Requests;

public class UploadPdfRequests
{
    [Required] public IFormFile FormFile { get; set; }

    [Required] public string FileName { get; set; }
}