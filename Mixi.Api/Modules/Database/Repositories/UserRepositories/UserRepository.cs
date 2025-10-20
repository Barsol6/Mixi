using Microsoft.EntityFrameworkCore;
using Mixi.Api.Modules.Users;

namespace Mixi.Api.Modules.Database.Repositories.UserRepositories;

public class UserRepository : IUserRepository
{
    private readonly MSSQLMixiDbContext _dbContext;

    public UserRepository(MSSQLMixiDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<User> AddUserAsync(User user)
    {
        _dbContext.Users.Add(user);
        await _dbContext.SaveChangesAsync();
        return user;
    }
    
    public bool CheckIfExists(string username)
    {
        var exists = _dbContext
            .Users.Any(x => x.Username == username);
        Console.WriteLine(exists);
        return exists;
    }
    public async Task<User?> GetUserAsync(int id)
    {
        var user = await _dbContext
            .Users.Where(u => u.Id == id)
            .FirstOrDefaultAsync();
        return user;
    } 
    
    public async Task<User?> GetUserAsync(string username)
    {
        var user = await _dbContext
            .Users.Where(u => u.Username == username)
            .FirstOrDefaultAsync();
        return user;
    }
    
    
    
}