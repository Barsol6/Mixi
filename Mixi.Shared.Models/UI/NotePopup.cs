namespace Mixi.Shared.Models.UI;

public class NotePopup
{
    private bool _isVisible;


    private string _noteId;
    private string _noteName;

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

    public event Action? NoteNameChanged;

    public event Action? NoteIdChanged;

    public event Action? IsVisibleChange;
}