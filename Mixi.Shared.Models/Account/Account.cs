namespace Mixi.Shared.Models.Account;

public class Account
{
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string PasswordRepeat { get; set; } = string.Empty;

    public string UserType { get; set; } = string.Empty;
}