using System.Collections.Generic;
using System.Linq;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Wordprocessing;
using Docx.DataModel;
using Docx.Processors.Searching;

namespace Docx.Processors
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
                            .Skip(svt.Token.Position.ParagraphIndex)
                            .ToArray();

                        startTextIndex = endOfText;
                        break;

                    case ArrayTemplate at:
                        var lastParagraph = this.ProcessArrayTemplate(at, paragraphs, context);
                        paragraphs = parent
                            .ChildElements
                            .OfType<Paragraph>()
                            .SkipWhile(p => p != lastParagraph)
                            .Skip(1)
                            .ToArray();

                        break;
                }
            } while (template != Template.Empty);
        }

        private int ProcessTemplate(SingleValueTemplate template, IReadOnlyCollection<Paragraph> paragraphs, Model context)
        {
            var p = paragraphs.ElementAt(template.Token.Position.ParagraphIndex);
            var model = context.Find(template.Token.ModelDescription.Expression);

            var textEndIndex = p.ReplaceToken(template.Token, model);
            return textEndIndex;
        }

        private Paragraph ProcessArrayTemplate(
            ArrayTemplate template,
            IReadOnlyCollection<Paragraph> paragraphs,
            Model context)
        {
            var collection = (CollectionModel)context.Find(template.Start.ModelDescription.Expression);
            var startParagraph = paragraphs.ElementAt(template.Start.Position.ParagraphIndex);
            var endParagraph = paragraphs.ElementAt(template.End.Position.ParagraphIndex);

            if (startParagraph != endParagraph)
            {
                var s = startParagraph.NextSibling();
                while(s != endParagraph)
                {
                    var t = s;
                    s = t.NextSibling();
                    t.Remove();
                }
            }

            var compositeProcessor = new CompositeElementProcessor(_engineConfig);
            var result = new List<OpenXmlElement>();

            foreach (var item in collection.Items)
            {
                var itemBody = template.OpenXml.CreateBody();
                compositeProcessor.Process(itemBody, item);

                result.AddRange(itemBody.ChildElements.Select(e => e.CloneNode(true)));
            }

            startParagraph.ReplaceToken(template.Start, Model.Empty);
            endParagraph.ReplaceToken(template.End, Model.Empty);

            foreach (var e in result)
            {
                endParagraph.InsertBeforeSelf(e);
            }

            return endParagraph;
        }
    }
}
