namespace Mixi.Shared.Models.UI;

public class NameGeneratorPopup
{
    private string? _generatedName = string.Empty;
    public event Action? NameChanged;

    public string? GeneratedName
    {
        get => _generatedName;
        set
        {
            if (_generatedName!=value)
            {
                _generatedName = value;
                NameChanged?.Invoke();
            }
        }
    }
    
}

