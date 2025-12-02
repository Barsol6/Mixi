using iTextSharp.text.pdf;

namespace Mixi.Api.Modules.Pdf;

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
                    var field = acroFields.GetFieldItem(fieldName);
                    if (field == null) continue;

                    var fieldDict = field.GetMerged(0);
                    if (fieldDict == null) continue;

                    var flags = fieldDict.GetAsNumber(PdfName.FF);

                    if (flags == null) continue;

                    var currentFlags = flags.IntValue;
                    var newFlags = currentFlags & ~readOnlyFlag;

                    fieldDict.Put(PdfName.FF, new PdfNumber(newFlags));
                }
            }

            stamper.Close();
            reader.Close();

            return outputStream.ToArray();
        }
    }
}