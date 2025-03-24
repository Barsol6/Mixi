namespace mixi.Modules.UI;

public class PdfPopup
{
    private string _pdfName = String.Empty;
    
    public event Action? PdfNameChanged;
    
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
}