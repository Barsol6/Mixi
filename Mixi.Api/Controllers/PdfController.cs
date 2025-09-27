using Microsoft.AspNetCore.Mvc;
using Mixi.Api.Modules.Pdf;
using Mixi.Api.Modules.Database;
using Mixi.Api.Modules.Database.Repositories.PdfRepositories;
using System.Collections.Generic;
using System.Linq;

namespace Mixi.Api.Controllers;

    [ApiController]
    [Route("api/pdfs/[controller]")]
    public class PdfController : ControllerBase
    {
        private readonly IPdfRepository _pdfRepository;
        
        public PdfController(IPdfRepository pdfRepository)
        {
            _pdfRepository = pdfRepository;
        }
        
        [HttpPost ("upload")]
        public async Task<IActionResult> UploadPdf([FromForm] IFormFile formFile, [FromForm] string fileName)
        {
            if (formFile == null || formFile.Length == 0 )
            {
                return BadRequest("No file uploaded");
            }

            if (formFile.ContentType != "application/pdf")
            {
                return BadRequest("File is not a PDF");
            }

            try
            {
                using var memoryStream = new MemoryStream();
                await formFile.CopyToAsync(memoryStream);
                var fileBytes = memoryStream.ToArray();

                var document = new PdfDocument
                {
                    Name = fileName,
                    Content = fileBytes,
                    FormData = "{}"
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
        public async Task<IActionResult> GetPdf(int id)
        {
            var pdfContent = await _pdfRepository.GetFileContentAsync(id);

            if (pdfContent == null)
            {
                return NotFound();
            }

            var unlockedContent = PdfHelper.UnlockPdfFormFields(pdfContent);
            
            return File(unlockedContent, "application/pdf");
        }
        
        [HttpGet ("{id}/getformdata")]
        public async Task<IActionResult> GetPdfFormData(int id)
        {
            var document = await _pdfRepository.GetByIdAsync(id);

            if (document==null)
            {
                return NotFound();
            }
            
            return Ok(document.FormData);
        }
        
        [HttpPut ("{id}/updateformdata")]
        public async Task<IActionResult> UpdatePdfFormData(int id, [FromBody] FormDataUpdateDto dto)
        {

            if (dto == null || string.IsNullOrWhiteSpace(dto.Data))
            {
                return BadRequest("Form data is missing or empty");
            }
            await _pdfRepository.UpdateFormDatasAsync(id, dto.Data);

            return NoContent();
        }


        [HttpGet ("getlist")]
        public async Task<ActionResult<IEnumerable<PdfListItemDto>>> GetPdfList()
        {
            var documents = await _pdfRepository.GetAllAsync();
            
            if (documents == null)
            {
                return Ok(new List<PdfListItemDto>());
            }
            
            return Ok(documents.Select(doc => new PdfListItemDto
            {
                Id = doc.Id,
                Name = doc.Name
            }).ToList());
        }
        
        [HttpDelete("{id}/delete")]
        public async Task<IActionResult> DeletePdf(int id)
        {
            var success = await _pdfRepository.DeleteAsync(id);

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
            public int Id { get; set; }
            public string Name { get; set; }
        }
    }
