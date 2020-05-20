using System.Collections.Generic;
using System.Linq;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Wordprocessing;

namespace Docx
{
    internal static class OpenXmlExtensions
    {
        public static IEnumerable<T> Childs<T>(this OpenXmlCompositeElement parent) => parent
                .ChildElements
                .OfType<T>();

        public static IEnumerable<Paragraph> Paragraphs(this OpenXmlCompositeElement parent) => parent
                .ChildElements
                .OfType<Paragraph>();

        public static IEnumerable<Run> Runs(this Paragraph paragraph) => paragraph
                .ChildElements
                .OfType<Run>();

        public static void RemoveSelfFromParent<T>(this IEnumerable<T> children)
            where T: OpenXmlElement
        {
            foreach(var c in children)
            {
                c.Remove();
            }
        }
    }
}
