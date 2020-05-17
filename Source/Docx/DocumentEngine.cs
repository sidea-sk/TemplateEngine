using System.IO;
using DocumentFormat.OpenXml.Packaging;
using Docx.DataModel;
using Docx.Processors;

namespace Docx
{
    public class DocumentEngine
    {
        private EngineConfig _engineConfig;

        public DocumentEngine() : this(EngineConfig.Default)
        {
        }

        public DocumentEngine(EngineConfig engineConfig)
        {
            _engineConfig = engineConfig;
        }

        public byte[] Run(Stream docxTemplate, IModel model)
            => this.Run(docxTemplate, model, _engineConfig);

        public byte[] Run(Stream docxTemplate, IModel model, EngineConfig engineConfig)
        {
            using var ms = new MemoryStream();
            docxTemplate.CopyTo(ms);

            using var docx = WordprocessingDocument.Open(ms, true);
            DocumentProcessor.Process(docx, model);

            return ms.ToArray();
        }

        public byte[] Run(byte[] docxTemplate, IModel model)
            => this.Run(docxTemplate, model);

        public byte[] Run(byte[] docxTemplate, IModel model, EngineConfig engineConfig)
            => this.Run(new MemoryStream(docxTemplate), model, engineConfig);
    }
}
