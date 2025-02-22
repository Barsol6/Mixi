using Microsoft.EntityFrameworkCore;
using mixi.Modules.Users;

namespace mixi.Modules.Database;

public class UserRepository : IUserRepository
{
    private readonly MixiDbContext _dbContext;

    public UserRepository(MixiDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<User> AddUserAsync(User user)
    {
        _dbContext.Users.Add(user);
        await _dbContext.SaveChangesAsync();
        return user;
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