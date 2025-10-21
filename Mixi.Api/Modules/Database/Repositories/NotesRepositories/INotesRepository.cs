using Mixi.Api.Modules.Notes;

namespace Mixi.Api.Modules.Database.Repositories.NotesRepositories;

public interface INotesRepository
{
    Task<List<Note>> GetAllAsync(string userName);
    Task<Note> GetByIdAsync(string id);
    Task<string> SaveAsync(Note note);
    Task<bool> DeleteAsync(string id);
}