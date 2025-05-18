

namespace mixi.Modules.UI;

public class SignUpPopup
{
 private bool _isVisible;
 public event Action? IsVisibleChange;
 public event Action? IsLoggedChange;

 private bool _isLogged;
 public bool IsVisible
 {
  get => _isVisible;
  set
  {
   if (_isVisible != value)
   {
    _isVisible = value;
    IsVisibleChange?.Invoke();
   }
  }
 }
 
 public bool IsLogged
 {
  get => _isLogged;
  set
  {
   if (_isLogged != value)
   {
    _isLogged = value;
    IsLoggedChange?.Invoke();
   }
  }
 }
 public string Username { get; set; } = String.Empty;
 public string? Password { get; set; } = String.Empty;
 public string PasswordRepeat { get; set; } = String.Empty;

 public string UserType { get; set; } = String.Empty;
}