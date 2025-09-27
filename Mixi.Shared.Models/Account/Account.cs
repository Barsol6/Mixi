namespace Mixi.Shared.Models.Account;

public class Account
{
    public string Username { get; set; } = String.Empty;
    public string Password { get; set; } = String.Empty;
    public string PasswordRepeat { get; set; } = String.Empty;

    public string UserType { get; set; } = String.Empty;
}