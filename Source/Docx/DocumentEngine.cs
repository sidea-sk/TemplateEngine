using System.IO;
using DocumentFormat.OpenXml.Packaging;
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

        public byte[] Run(Stream docxTemplate)
            => this.Run(docxTemplate, _engineConfig);

        public byte[] Run(Stream docxTemplate, EngineConfig engineConfig)
        {
            using var ms = new MemoryStream();
            docxTemplate.CopyTo(ms);

            using var docx = WordprocessingDocument.Open(ms, true);
            DocumentProcessor.Process(docx);

            return ms.ToArray();
        }

        public byte[] Run(byte[] docxTemplate)
            => this.Run(docxTemplate);

        public byte[] Run(byte[] docxTemplate, EngineConfig engineConfig)
            => this.Run(new MemoryStream(docxTemplate), engineConfig);
    }
}
