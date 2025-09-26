namespace Mixi.Api.Modules.UI;

public class PdfPopup
{
    private int _pdfName;
    
    public event Action? PdfNameChanged;
    
    public int PdfName
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