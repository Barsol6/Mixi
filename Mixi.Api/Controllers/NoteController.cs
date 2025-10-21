using Microsoft.AspNetCore.Mvc;
using Mixi.Api.Modules.Database.Repositories.NotesRepositories;
using Mixi.Api.Modules.Notes;
using Mixi.Api.Requests;

namespace Mixi.Api.Controllers;

[ApiController]
[Route("api/[controller]")]

public class NoteController : ControllerBase
{
    private readonly INotesRepository _notesRepository;

    public NoteController(INotesRepository notesRepository)
    {
        _notesRepository = notesRepository;
    }

    [HttpPost("{id}/create")]
    public async Task<IActionResult> CreateNote([FromForm] CreateNoteRequest request, string id)
    {

        try
        {
            var note = new Note
            {
                Text = request.NoteText,
                CreatedAt = DateTime.Now,
                UserName = id,
                UpdatedAt = DateTime.Now,
                Name = request.NoteName
            };

            var newId = await _notesRepository.SaveAsync(note);

            return Ok(new { id = newId });
        }
        catch (Exception e)
        {
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
            Name = note.Name
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
    
    
    public class NoteListItemDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }
}