using System.ComponentModel;

namespace mixi.Modules.UI;

public class SignUpPopup
{
 private bool _isVisible;
 public event Action? IsVisibleChange;
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
 public string Username { get; set; } = String.Empty;
 public string Password { get; set; } = String.Empty;
 public string PasswordRepeat { get; set; } = String.Empty;

 public void dupa()
 {
  Console.Out.Write(IsVisible);
 }
}