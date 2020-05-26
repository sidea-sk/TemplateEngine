using System.Collections.Generic;
using DocumentFormat.OpenXml;
using Docx.DataModel;

namespace Docx.Processors
{
    internal class CompositeElementProcessor
    {
        private ParagraphsProcessor _paragraphsProcessor;
        private TablesProcessor _tablesProcessor;

        public CompositeElementProcessor(EngineConfig engineConfig)
        {
            _paragraphsProcessor = new ParagraphsProcessor(engineConfig);
            _tablesProcessor = new TablesProcessor(engineConfig);
        }

        public void Process(OpenXmlCompositeElement compositeElement, Model context)
        {
            _paragraphsProcessor.Process(compositeElement, context);
            _tablesProcessor.Process(compositeElement, context);
        }
    }
}
