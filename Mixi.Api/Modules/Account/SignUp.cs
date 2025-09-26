using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Mixi.Api.Modules.Database.Repositories.UserRepositories;
using Mixi.Api.Modules.UI;
using Mixi.Api.Modules.Users;

namespace Mixi.Api.Modules.Account;
public class SignUp
{
    
    public SignUp(SignUpPopup signUps, PasswordHash passwordHash, IUserRepository userRepository, ProtectedSessionStorage storage)
    {
        SignUps = signUps;
        PasswordHash = passwordHash;
        UserRepository = userRepository;
        Storage = storage;
    }

    [Inject] private ProtectedSessionStorage Storage { get; set; }
    [Inject] private IUserRepository UserRepository { get; set; }
    [Inject] private PasswordHash PasswordHash { get; set; }
    [Inject] public SignUpPopup SignUps { get; set; }
    
    
    
    public async Task<bool> CreateAccount()
    {
        SignUps.Password = PasswordHash.HashPasswords(SignUps.Password, SignUps.Username);
        var user = new User { Username = SignUps.Username, Password = SignUps.Password, UserType = SignUps.UserType};
        var exists = UserRepository.CheckIfExists(SignUps.Username);
        Console.WriteLine(exists);
        if (exists is true)
        {
            Console.WriteLine("Account exists");
            SignUps.Username = String.Empty;
            SignUps.Password = String.Empty;
            SignUps.PasswordRepeat = String.Empty;
            return false;
        }
        await UserRepository.AddUserAsync(user);
        SignUps.IsVisible = false;
        await Storage.SetAsync("SignUpPopupIsVisible", SignUps.IsVisible);
        SignUps.Username = String.Empty;
        SignUps.Password = String.Empty;
        SignUps.PasswordRepeat = String.Empty;
        return true;
    }
    


}