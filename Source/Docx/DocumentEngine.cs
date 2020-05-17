using System.IO;
using DocumentFormat.OpenXml.Packaging;

namespace Docx
{
    public class DocumentEngine
    {
        public MemoryStream RunAsStream(Stream docxStream)
            => this.RunAsStream(docxStream, EngineConfig.Default);

        public MemoryStream RunAsStream(Stream docxStream, EngineConfig engineConfig)
        {
            using var docx = WordprocessingDocument.Open(docxStream, false);

            return new MemoryStream();
        }

        public byte[] Run(byte[] docxTemplate)
            => this.Run(docxTemplate, EngineConfig.Default);

        public byte[] Run(byte[] docxTemplate, EngineConfig engineConfig)
        {
            using var ms = this.RunAsStream(new MemoryStream(docxTemplate), engineConfig);
            return ms.ToArray();
        }
    }
}
