using System.Collections.Generic;
using DocumentFormat.OpenXml;
using Docx.DataModel;

namespace Docx.Processors
{
    internal class CompositeElementProcessor
    {
        private ParagraphsProcessor _paragraphsProcessor;

        public CompositeElementProcessor(EngineConfig engineConfig)
        {
            _paragraphsProcessor = new ParagraphsProcessor(engineConfig);
        }

        public void Process(OpenXmlCompositeElement compositeElement, Model context)
        {
            _paragraphsProcessor.Process(compositeElement, context);
            // _tableProcess.Process(compositeElement, context);
        }
    }
}
