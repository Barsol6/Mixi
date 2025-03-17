using System.Runtime.InteropServices;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using mixi.Components.Popups;
using mixi.Components.UI;
using mixi.Modules.Database;
using mixi.Modules.UI;
using mixi.Modules.Users;

namespace mixi.Modules.Account;
public class SignUp
{
    private ProtectedSessionStorage Storage;
    public SignUp(SignUpPopup signUps, PasswordHash passwordHash, IUserRepository userRepository)
    {
        SignUps = signUps;
        PasswordHash = passwordHash;
        UserRepository = userRepository;
    }

    
    [Inject] private IUserRepository UserRepository { get; set; }
    [Inject] private PasswordHash PasswordHash { get; set; }
    [Inject] public SignUpPopup SignUps { get; set; }
    
    
    public Task CreateAccount()
    {
        SignUps.Password = PasswordHash.HashPasswords(SignUps.Password, SignUps.Username);
        var user = new User { Username = SignUps.Username, Password = SignUps.Password, UserType = SignUps.UserType}; 
        UserRepository.AddUserAsync(user);
        SignUps.IsVisible = false;
        Storage.SetAsync("SignUpPopupIsVisible", SignUps.IsVisible);
        SignUps.Username = String.Empty;
        SignUps.Password = String.Empty;
        SignUps.PasswordRepeat = String.Empty;
        return Task.CompletedTask;
    }
    


}