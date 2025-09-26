namespace Mixi.Api.Modules.UI;

public class MenuPopup
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
}