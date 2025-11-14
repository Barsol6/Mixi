using Microsoft.AspNetCore.Components;
using Mixi.Api.Modules.Database.Repositories.UserRepositories;
using Mixi.Api.Modules.Users;

namespace Mixi.Api.Modules.Account;
public class SignUp
{
    
    public SignUp(PasswordHash passwordHash, IUserRepository userRepository)
    {
        PasswordHash = passwordHash;
        UserRepository = userRepository;
    }
    
    [Inject] private IUserRepository UserRepository { get; set; }
    [Inject] private PasswordHash PasswordHash { get; set; }
    
    
    
    
    
    public async Task<bool> CreateAccount(string username, string password, string repeatpassword, string userType)
    {
        var hashedPassword = PasswordHash.HashPasswords(password, username);
        
        var exists = UserRepository.CheckIfExists(username);
        if (exists is true)
        {
            Console.WriteLine("Account exists");
            return false;
        }
        
        var user = new User
        {
            Username = username, 
            Password = password, 
            UserType = userType
        };
     
        await UserRepository.AddUserAsync(user);
        return true;
    }
    


}