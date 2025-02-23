using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using mixi.Modules.Database;
using mixi.Modules.Users;

namespace mixi.Modules.Hashers;

public  class PasswordHash
{
    
    private PasswordHasher<object> _passwordHasher = new PasswordHasher<object>();
    private string _hashedPassword = String.Empty;

    private User _user = new();

    public PasswordHash(IUserRepository userRepository)
    {
        UserRepository = userRepository;
    }

    [Inject] private IUserRepository UserRepository { get; set; }
   

    public string HashPasswords(string password, string username)
    {
       _hashedPassword = _passwordHasher.HashPassword(username, password);

       return _hashedPassword;
    }

    public async Task<bool> CheckPassword(string password, string username)
    {
        
        var user = await UserRepository.GetUserAsync(username);
        
        if (user is null)
        {
            user = new User { Username = username, Password = HashPasswords(username, password)};
            await UserRepository.AddUserAsync(user);
        }
         _hashedPassword = user.Password;
        

         var passwordCheck = _passwordHasher.VerifyHashedPassword(username, _hashedPassword, password);
         Console.Out.Write(passwordCheck);
         return passwordCheck switch
         {
             PasswordVerificationResult.Failed => false,
             PasswordVerificationResult.Success => true,
             _ => false
         };
    }
}