using mixi.Modules.Users;

namespace mixi.Modules.Database;

public interface IUserRepository
{
    public Task<User> AddUserAsync(User user);

    public Task<User?> GetUserAsync(int id);

    public Task<User?> GetUserAsync(string username);

    public bool CheckIfExists(string username);
}