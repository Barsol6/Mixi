using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Mixi.Api.Modules.Database.Repositories.UserRepositories;
using Mixi.Api.Modules.Users;
using Mixi.Shared.Models.UI;

namespace Mixi.Api.Modules.Account;
public class SignUp
{
    
    public SignUp(SignUpPopup signUps, PasswordHash passwordHash, IUserRepository userRepository, ProtectedSessionStorage storage, Shared.Models.Account.Account account)
    {
        SignUps = signUps;
        PasswordHash = passwordHash;
        UserRepository = userRepository;
        Storage = storage;
        Account = account;
    }

    [Inject] private ProtectedSessionStorage Storage { get; set; }
    [Inject] private IUserRepository UserRepository { get; set; }
    [Inject] private PasswordHash PasswordHash { get; set; }
    [Inject] public SignUpPopup SignUps { get; set; }
    
    [Inject] public Shared.Models.Account.Account Account { get; set; }
    
    
    
    
    public async Task<bool> CreateAccount()
    {
        Account.Password = PasswordHash.HashPasswords(Account.Password, Account.Username);
        var user = new User { Username = Account.Username, Password = Account.Password, UserType = Account.UserType};
        var exists = UserRepository.CheckIfExists(Account.Username);
        Console.WriteLine(exists);
        if (exists is true)
        {
            Console.WriteLine("Account exists");
            Account.Username = String.Empty;
            Account.Password = String.Empty;
            Account.PasswordRepeat = String.Empty;
            return false;
        }
        await UserRepository.AddUserAsync(user);
        SignUps.IsVisible = false;
        await Storage.SetAsync("SignUpPopupIsVisible", SignUps.IsVisible);
        Account.Username = String.Empty;
        Account.Password = String.Empty;
        Account.PasswordRepeat = String.Empty;
        return true;
    }
    


}