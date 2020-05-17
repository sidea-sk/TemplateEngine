using System.Linq;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Wordprocessing;
using Docx.DataModel;

namespace Docx.Processors
{
    internal static class ParagraphsProcessor
    {
        public static void Process(OpenXmlCompositeElement parent, IModel context)
        {
            var paragraphs = parent
                .ChildElements
                .OfType<Paragraph>()
                .ToArray();

            var currentParagraphIndex = 0;
            var currentTextIndex = 0;
            do
            {

            } while (currentParagraphIndex < paragraphs.Length);


        }
    }
}
