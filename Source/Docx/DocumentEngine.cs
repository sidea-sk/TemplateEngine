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

        public byte[] Run(Stream docxTemplate, Model model)
            => this.Run(docxTemplate, model, _engineConfig);

        public byte[] Run(Stream docxTemplate, Model model, EngineConfig engineConfig)
        {
            var processor = new DocumentProcessor(engineConfig);
            using (var ms = new MemoryStream())
            {
                docxTemplate.CopyTo(ms);

                using (var docx = WordprocessingDocument.Open(ms, true))
                {
                    processor.Process(docx, model);
                }

                return ms.ToArray();
            }
        }

        public byte[] Run(byte[] docxTemplate, Model model)
            => this.Run(docxTemplate, model);

        public byte[] Run(byte[] docxTemplate, Model model, EngineConfig engineConfig)
            => this.Run(new MemoryStream(docxTemplate), model, engineConfig);
    }
}
