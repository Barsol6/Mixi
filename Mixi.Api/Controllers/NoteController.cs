using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Mixi.Api.Modules.Database.Repositories.NotesRepositories;
using Mixi.Api.Modules.Notes;
using Mixi.Api.Requests;

namespace Mixi.Api.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]

public class NoteController : ControllerBase
{
    private readonly INotesRepository _notesRepository;

    public NoteController(INotesRepository notesRepository)
    {
        _notesRepository = notesRepository;
    }
    
    private string? GetUserName()
    {
        return User.FindFirst(ClaimTypes.Name)?.Value;
    }

    [HttpPost("create")]
    public async Task<IActionResult> CreateNote([FromForm] CreateNoteRequest request)
    {
        var username = GetUserName();
        try
        {
            var note = new Note
            {
                Text = String.Empty,
                CreatedAt = DateTime.Now,
                UserName = username,
                UpdatedAt = DateTime.Now,
                Name = request.NoteName,
            };

            var newId = await _notesRepository.SaveAsync(note);
            
            return Ok(new { id = newId });
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(500, $"Internal server error: {e.Message}");
        }
    }

    [HttpGet("{id}/content")]
    public async Task<IActionResult> GetNote(string id)
    {
        var note = await _notesRepository.GetByIdAsync(id);
        
        if (note == null)
        {
            return NotFound();
        }
        
        return Ok(note.Text);
    }
    
    [HttpGet("{id}/getlist")]
    public async Task<ActionResult<IEnumerable<NoteListItemDto>>> GetNotes(string id)
    {
        var notes = await _notesRepository.GetAllAsync(id);
        
        if (notes == null)
        {
            return Ok(new List<NoteListItemDto>());
        }
        
        var notesList = notes.Select(note => new NoteListItemDto
        {
            Id = note.Id,
            Name = note.Name ?? "No name"
        }).ToList();
        
        
        return Ok(notesList);
    }

    [HttpDelete("{id}/delete")]
    public async Task<IActionResult> DeleteNote(string id)
    {
        var success = await _notesRepository.DeleteAsync(id);

        if (!success)
        {
            return NotFound($"Note with ID {id} not found");
        }
        
        return NoContent();
    }

    [HttpPut("save")]
    public async Task<IActionResult> SaveNote([FromBody] NoteDataUpdate note)
    {
        var _userName = GetUserName();
        
        await _notesRepository.SaveAsync(_userName, note.Data, note.Name);
        return Ok();
    }
    
    
    public class NoteListItemDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }

    public class NoteDataUpdate
    {
        public string Data { get; set; }
        
        public string? Name { get; set; }
    }
}