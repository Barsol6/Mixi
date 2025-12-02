using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Mixi.Api.Modules.Database.Repositories.NotesRepositories;
using Mixi.Api.Modules.Notes;
using Mixi.Api.Requests;
using MongoDB.Bson;

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

        if (username == null) return Unauthorized();

        try
        {
            var note = new Note
            {
                Text = string.Empty,
                CreatedAt = DateTime.Now,
                UserName = username,
                UpdatedAt = DateTime.Now,
                Name = request.NoteName
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
        if (!ObjectId.TryParse(id, out _)) return BadRequest("Invalid ID format");

        var userId = GetUserName();

        if (userId == null) return Unauthorized();

        var note = await _notesRepository.GetByIdAsync(id, userId);

        if (note == null) return NotFound();

        return Ok(note.Text);
    }

    [HttpGet("getlist")]
    public async Task<ActionResult<IEnumerable<NoteListItemDto>>> GetNotes()
    {
        var id = GetUserName();
        if (id == null) return Unauthorized();

        var notes = await _notesRepository.GetAllAsync(id);

        if (notes == null) return Ok(new List<NoteListItemDto>());

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
        if (!ObjectId.TryParse(id, out _)) return BadRequest("Invalid ID format");

        var userId = GetUserName();

        if (userId == null) return Unauthorized();

        var success = await _notesRepository.DeleteAsync(id, userId);

        if (!success) return NotFound($"Note with ID {id} not found");

        return NoContent();
    }

    [HttpPut("save")]
    public async Task<IActionResult> SaveNote([FromBody] NoteDataUpdate note)
    {
        var userName = GetUserName();

        if (userName == null) return Unauthorized();

        await _notesRepository.SaveAsync(userName, note.NoteData, note.NoteName, note.NoteId);
        return Ok();
    }


    public class NoteListItemDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }

    public class NoteDataUpdate
    {
        public string NoteData { get; set; }

        public string NoteId { get; set; }

        public string NoteName { get; set; }
    }
}