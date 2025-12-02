namespace Mixi.Shared.Models.UI;

public class PdfPopup
{
    private bool _isVisible;
    private string _pdfName;

    public string PdfName
    {
        get => _pdfName;
        set
        {
            if (_pdfName != value)
            {
                _pdfName = value;
                PdfNameChanged?.Invoke();
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

    public event Action? PdfNameChanged;

    public event Action? IsVisibleChange;
}