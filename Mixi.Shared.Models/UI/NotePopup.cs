namespace Mixi.Shared.Models.UI;

public class NotePopup
{
    private string _noteName;
    
    public event Action? NoteNameChanged;
    
    public string NoteName
    {
        get => _noteName;
        set
        {
            if (_noteName != value)
            {
                _noteName = value;
                NoteNameChanged?.Invoke();
            }
        }
    }
    

    private string _noteId;
    
    public event Action? NoteIdChanged;
    
    public string NoteId
    {
        get => _noteId;
        set
        {
            if (_noteId != value)
            {
                _noteId = value;
                NoteIdChanged?.Invoke();
            }
        }
    }
    
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