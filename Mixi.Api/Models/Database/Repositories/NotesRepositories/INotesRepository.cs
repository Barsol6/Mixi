using Mixi.Api.Modules.Notes;

namespace Mixi.Api.Modules.Database.Repositories.NotesRepositories;

public interface INotesRepository
{
    Task<List<Note>> GetAllAsync(string userName);
    Task<Note?> GetByIdAsync(string id, string userName);
    Task<string> SaveAsync(Note note);
    Task<bool> DeleteAsync(string id, string userName);
    Task<bool> SaveAsync(string userName, string noteData, string noteName, string noteId);
}