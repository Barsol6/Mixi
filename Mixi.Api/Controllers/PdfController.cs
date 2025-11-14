using Microsoft.AspNetCore.Mvc;
using Mixi.Api.Modules.Pdf;
using Mixi.Api.Modules.Database;
using Mixi.Api.Modules.Database.Repositories.PdfRepositories;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Mixi.Api.Requests;
using MongoDB.Bson;

namespace Mixi.Api.Controllers;

    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class PdfController : ControllerBase
    {
        private readonly IPdfRepository _pdfRepository;
        
        public PdfController(IPdfRepository pdfRepository)
        {
            _pdfRepository = pdfRepository;
        }

        private string? GetUserName()
        {
            return User.FindFirst(ClaimTypes.Name)?.Value;
        }
        
        [HttpPost ("upload")]
        public async Task<IActionResult> UploadPdf(IFormFile FormFile, [FromForm] string FileName)
        {
            
            if (FormFile.Length > 20 * 1024 * 1024) 
            {
                return BadRequest("File size exceeds limit");
            }
            
            if (FormFile == null || FormFile.Length == 0 )
            {
                return BadRequest("No file uploaded");
            }

            if (FormFile.ContentType != "application/pdf")
            {
                return BadRequest("File is not a PDF");
            }

            try
            {
                using var memoryStream = new MemoryStream();
                await FormFile.CopyToAsync(memoryStream);
                var fileBytes = memoryStream.ToArray();

                var document = new PdfDocument
                {
                    FileName = FileName,
                    FileContent = fileBytes,
                    FormData = "{}",
                    UserName = GetUserName(),
                };

                var newId = await _pdfRepository.SaveAsync(document, fileBytes);
                
                return Ok(new { id = newId });
            }
            catch (Exception e)
            {
                return StatusCode(500, $"Internal server error: {e.Message}");
            }
        }
        
        [HttpGet ("{id}/content")]
        public async Task<IActionResult> GetPdf(string id)
        {
            if (!ObjectId.TryParse(id, out _))
            {
                return BadRequest("Invalid ID format");
            }
            
            var username = GetUserName();

            if (username == null)
            {
                return Unauthorized();
            }
            
            var pdfContent = await _pdfRepository.GetFileContentAsync(id, username);

            if (pdfContent == null)
            {
                return NotFound();
            }

            var unlockedContent = PdfHelper.UnlockPdfFormFields(pdfContent);
            
            return File(unlockedContent, "application/pdf");
        }
        
        [HttpGet ("{id}/getformdata")]
        public async Task<IActionResult> GetPdfFormData(string id)
        {
            if (!ObjectId.TryParse(id, out _))
            {
                return BadRequest("Invalid ID format");
            }
            
            var username = GetUserName();

            if (username == null)
            {
                return Unauthorized();
            }
            
            var document = await _pdfRepository.GetByIdAsync(id, username);

            if (document==null)
            {
                return NotFound();
            }
            
            return Ok(document.FormData);
        }
        
        [HttpPut ("{id}/updateformdata")]
        public async Task<IActionResult> UpdatePdfFormData(string id, [FromBody] FormDataUpdateDto dto)
        {
            if (!ObjectId.TryParse(id, out _))
            {
                return BadRequest("Invalid ID format");
            }
            
            var username = GetUserName();

            if (username  == null)
            {
                return Unauthorized();
            }

            if (dto == null || string.IsNullOrWhiteSpace(dto.Data))
            {
                return BadRequest("Form data is missing or empty");
            }
            await _pdfRepository.UpdateFormDatasAsync(id, dto.Data, username);

            return NoContent();
        }


        [HttpGet ("getlist")]
        public async Task<ActionResult<IEnumerable<PdfListItemDto>>> GetPdfList()
        {
            var username = GetUserName();
            var documents = await _pdfRepository.GetAllAsync(username);
            
            if (documents == null)
            {
                return Ok(new List<PdfListItemDto>());
            }
            
            var documentss = documents.Select(doc => new PdfListItemDto
            {
                Id = doc.Id,
                Name = doc.FileName ?? "No name"
            }).ToList();
            
            return Ok(documentss);
        }
        
        [HttpDelete("{id}/delete")]
        public async Task<IActionResult> DeletePdf(string id)
        {
            if (!ObjectId.TryParse(id, out _))
            {
                return BadRequest("Invalid ID format");
            }
            
            var username = GetUserName();

            if (username == null)
            {
                return Unauthorized();
            }
            
            var success = await _pdfRepository.DeleteAsync(id, username);

            if (!success)
            {
                return NotFound($"Document with ID {id} not found");
            }
            
            return NoContent();
        }
        
        public class FormDataUpdateDto
        {
            public string Data { get; set; }
        }
        
        public class PdfListItemDto
        {
            public string Id { get; set; }
            public string Name { get; set; }
        }
    }
