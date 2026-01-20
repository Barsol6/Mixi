namespace Mixi.Shared.Models.UI;

public class MenuPopup
{
    private bool _isVisible;

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

    public event Action? IsVisibleChange;
}