using DocumentFormat.OpenXml.Packaging;
using Docx.DataModel;

namespace Docx.Processors
{
    internal class DocumentProcessor
    {
        public static void Process(WordprocessingDocument document, IModel documentModel)
        {
            var mainPart = document.MainDocumentPart;
            CompositeElementProcessor.Process(mainPart.Document.Body, documentModel);

            foreach (var hp in mainPart.HeaderParts)
            {
                CompositeElementProcessor.Process(hp.Header, documentModel);
            }

            foreach (var fp in mainPart.FooterParts)
            {
                CompositeElementProcessor.Process(fp.Footer, documentModel);
            }
        }
    }
}
