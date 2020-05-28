using DocumentFormat.OpenXml;
using Docx.DataModel;

namespace Docx.Processors
{
    internal class CompositeElementProcessor
    {
        private ParagraphsProcessor _paragraphsProcessor;
        private TablesProcessor _tablesProcessor;

        public CompositeElementProcessor(EngineConfig engineConfig, IImageProcessor imageProcessor)
        {
            _paragraphsProcessor = new ParagraphsProcessor(engineConfig, imageProcessor);
            _tablesProcessor = new TablesProcessor(engineConfig, imageProcessor);
        }

        public void Process(OpenXmlCompositeElement compositeElement, Model context)
        {
            _paragraphsProcessor.Process(compositeElement, context);
            _tablesProcessor.Process(compositeElement, context);
        }
    }
}
