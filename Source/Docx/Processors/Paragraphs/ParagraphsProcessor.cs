using System.Collections.Generic;
using System.Linq;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Wordprocessing;
using Docx.DataModel;
using Docx.Processors.Searching;

namespace Docx.Processors.Paragraphs
{
    internal class ParagraphsProcessor
    {
        private readonly EngineConfig _engineConfig;

        public ParagraphsProcessor(EngineConfig engineConfig)
        {
            _engineConfig = engineConfig;
        }

        public void Process(OpenXmlCompositeElement parent, Model context)
        {
            var paragraphs = parent
                .ChildElements
                .OfType<Paragraph>()
                .ToArray();

            Template template;
            int startTextIndex = 0;
            do
            {
                template = paragraphs.FindNextTemplate(startTextIndex, _engineConfig);

                switch (template)
                {
                    case SingleValueTemplate svt:
                        var endOfText = this.ProcessTemplate(svt, paragraphs, context);

                        paragraphs = paragraphs
                            .Skip(svt.Token.ParagraphIndex)
                            .ToArray();

                        startTextIndex = endOfText;
                        break;
                }
            } while (template != Template.Empty);
        }

        private int ProcessTemplate(SingleValueTemplate template, IReadOnlyCollection<Paragraph> paragraphs, Model context)
        {
            var p = paragraphs.ElementAt(template.Token.ParagraphIndex);
            var model = context.Find(template.Token.ModelDescription.Expression);

            var textEndIndex = p.ReplaceToken(template.Token, model);
            return textEndIndex;
        }
    }
}
