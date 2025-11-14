using System.Text.RegularExpressions;
using Mixi.Api.Modules.Notes;
using MongoDB.Driver;
using MongoDB.Bson;

namespace Mixi.Api.Modules.Database.Repositories.NotesRepositories;

public class NotesRepository:INotesRepository
{
    private readonly MongoMixiDbContext _context;
    private readonly ILogger<NotesRepository> _logger;


    public NotesRepository(MongoMixiDbContext context, ILogger<NotesRepository> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<Note?> GetByIdAsync(string id, string userName )
    {
        try
        {
            return await _context.NotesCollection.Find(p => p.Id == id && p.UserName == userName).FirstOrDefaultAsync();
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error while getting note by id");
            throw;
        }
    }
    
    public async Task<List<Note>> GetAllAsync(string userName)
    {
        try
        {
            var filter = Builders<Note>.Filter.Regex(
                x => x.UserName,
                new MongoDB.Bson.BsonRegularExpression($"^{Regex.Escape(userName)}$", "i")
            );
            return await _context.NotesCollection.Find(filter).ToListAsync();        
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error while getting all notes");
            throw;
        }
    }

    public async Task<bool> SaveAsync(string userName, string noteData, string noteName, string noteId)
    {
        try
        {
            var note = await _context.NotesCollection.Find(x => x.Id == noteId).FirstOrDefaultAsync();
            if (note is null)
            {
                _logger.LogError($"Note with id {noteId} not found");
                return false;
            }
            
            note.Text = noteData;
            note.Name = noteName;
            note.UpdatedAt = DateTime.UtcNow;
            await _context.NotesCollection.ReplaceOneAsync(x => x.Id == noteId, note);
            _logger.LogInformation($"Note with id {noteId} updated");
            return true;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error while updating note");
            throw;
        }
    }
    
    public async Task<string> SaveAsync(Note note)
    {
        try
        {
            var isNew = note.Id == string.Empty;
            if (!isNew)
            {
                var existingNote = await _context.NotesCollection.Find(x => x.Id == note.Id).FirstOrDefaultAsync();
                if (existingNote is null)
                {
                    throw new KeyNotFoundException($"Note with id {note.Id} not found");
                }

                existingNote.UpdatedAt = DateTime.UtcNow;
                existingNote.UserName = note.UserName;
                existingNote.Text = note.Text;
                existingNote.CreatedAt = note.CreatedAt;
                existingNote.Name = note.Name;

                await _context.NotesCollection.ReplaceOneAsync(x => x.Id == existingNote.Id, existingNote);
            }
            else
            {
                note.CreatedAt = DateTime.UtcNow;
                note.UpdatedAt = DateTime.UtcNow;
             
                
                await _context.NotesCollection.InsertOneAsync(note);
            }

            return note.Id;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error while saving note");
            throw;
        }
    }
    
    public async Task<bool> DeleteAsync(string id, string userName)
    {
        try
        {
            var note = await _context.NotesCollection.Find(x => x.Id == id && x.UserName == userName).FirstOrDefaultAsync();
            if (note is null)
            {
                _logger.LogError($"Note with id {id} not found");
                return false;
            }
            await _context.NotesCollection.DeleteOneAsync(x => x.Id == note.Id && x.UserName == userName);
            _logger.LogInformation($"Note with id {id} deleted");
            return true;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error while deleting note");
            throw;
        }
    }
    
    
}