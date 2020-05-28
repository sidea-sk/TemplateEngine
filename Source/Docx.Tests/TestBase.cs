using System.IO;
using System.Text;
using Docx.DataModel;

namespace Docx.Tests
{
    public abstract class TestBase
    {
        private readonly string _outputFolder;

        protected string SamplesFolder { get; }

        protected TestBase(string samplesSubFolder)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            SamplesFolder = $"../../../../Samples/{samplesSubFolder}";
            _outputFolder = $"../../../../TestOutputs/{samplesSubFolder}";
        }

        protected void Process(string docxSampleFileName, Model model)
            => this.Process(docxSampleFileName, model, EngineConfig.Default);

        protected void Process(string docxSampleFileName, Model model, EngineConfig config)
        {
            if (!Directory.Exists(_outputFolder))
            {
                Directory.CreateDirectory(_outputFolder);
            }

            var outputFileName = $"{_outputFolder}/{docxSampleFileName}.docx";
            if (File.Exists(outputFileName))
            {
                File.Delete(outputFileName);
            }

            var inputFileName = $"{SamplesFolder}/{docxSampleFileName}.docx";
            using var templateStream = File.Open(inputFileName, FileMode.Open, FileAccess.Read);

            var engine = new DocumentEngine(config);
            var docx = engine.Run(templateStream, model);

            File.WriteAllBytes(outputFileName, docx);
        }
    }
}
