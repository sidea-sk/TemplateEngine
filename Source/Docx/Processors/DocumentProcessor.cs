using DocumentFormat.OpenXml.Packaging;
using Docx.DataModel;

namespace Docx.Processors
{
    internal class DocumentProcessor
    {
        private CompositeElementProcessor _compositeElementProcessor;

        public DocumentProcessor(EngineConfig engineConfig)
        {
            _compositeElementProcessor = new CompositeElementProcessor(engineConfig);
        }

        public void Process(WordprocessingDocument document, Model documentModel)
        {
            var mainPart = document.MainDocumentPart;
            _compositeElementProcessor.Process(mainPart.Document.Body, documentModel);

            foreach (var hp in mainPart.HeaderParts)
            {
                _compositeElementProcessor.Process(hp.Header, documentModel);
            }

            foreach (var fp in mainPart.FooterParts)
            {
                _compositeElementProcessor.Process(fp.Footer, documentModel);
            }
        }
    }
}
