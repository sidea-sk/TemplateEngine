using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Docx.Tests
{
    public abstract class TestBase
    {
        private readonly string _samplesFolder;
        private readonly string _outputFolder;

        protected TestBase(string samplesSubFolder)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            _samplesFolder = $"../../../../Samples/{samplesSubFolder}";
            _outputFolder = $"../../../../TestOutputs/{samplesSubFolder}";
        }

        protected void Process(string docxSampleFileName)
            => this.Process(docxSampleFileName, EngineConfig.Default);

        protected void Process(string docxSampleFileName, EngineConfig config)
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

            var inputFileName = $"{_samplesFolder}/{docxSampleFileName}.docx";
            using var templateStream = File.Open(inputFileName, FileMode.Open, FileAccess.Read);

            var engine = new DocumentEngine(config);
            var docx = engine.Run(templateStream);

            File.WriteAllBytes(outputFileName, docx);
        }
    }
}
