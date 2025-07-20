using System.IO;
using iTextSharp.text.pdf;

namespace mixi.Modules.Pdf;

public static class PdfHelper
{
    public static byte[] UnlockPdfFormFields(byte[] pdfData)
    {
        using (var outputStream = new MemoryStream())
        {
            var reader = new PdfReader(pdfData);
            var stamper = new PdfStamper(reader, outputStream);
            var acroFields = stamper.AcroFields;

            stamper.FormFlattening = false;

            if (acroFields.Fields != null)
            {
                const int readOnlyFlag = 1;

                foreach (var fieldName in acroFields.Fields.Keys)
                {
   
                    AcroFields.Item field = acroFields.GetFieldItem(fieldName);
                    if (field == null) continue;

                    PdfDictionary fieldDict = field.GetMerged(0);
                    if (fieldDict == null) continue;
                    
                    PdfNumber flags = fieldDict.GetAsNumber(PdfName.FF);

                    if (flags == null) continue;
                    
                    int currentFlags = flags.IntValue;
                    int newFlags = currentFlags & ~readOnlyFlag;
                    
                    fieldDict.Put(PdfName.FF, new PdfNumber(newFlags));
                }
            }
            
            stamper.Close();
            reader.Close();

            return outputStream.ToArray();
        }
    }
}