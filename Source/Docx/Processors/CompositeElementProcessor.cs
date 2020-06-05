using DocumentFormat.OpenXml;
using Docx.DataModel;
using Microsoft.Extensions.Logging;

namespace Docx.Processors
{
    internal class CompositeElementProcessor
    {
        private ParagraphsProcessor _paragraphsProcessor;
        private TablesProcessor _tablesProcessor;

        public CompositeElementProcessor(EngineConfig engineConfig, IImageProcessor imageProcessor, ILogger logger)
        {
            _paragraphsProcessor = new ParagraphsProcessor(engineConfig, imageProcessor, logger);
            _tablesProcessor = new TablesProcessor(engineConfig, imageProcessor, logger);
        }

        public void Process(OpenXmlCompositeElement compositeElement, Model context)
        {
            _paragraphsProcessor.Process(compositeElement, context);
            _tablesProcessor.Process(compositeElement, context);
        }
    }
}
