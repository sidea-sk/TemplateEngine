using System.Collections.Generic;
using System.Linq;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Wordprocessing;

namespace Docx.Processors.Searching
{
    internal class OpenXmlTemplate
    {
        public static readonly OpenXmlTemplate Empty = new OpenXmlTemplate(new Run[0], new OpenXmlElement[0], new Run[0]);

        public OpenXmlTemplate(IEnumerable<OpenXmlElement> elements) : this(new Run[0], elements, new Run[0])
        {
        }

        public OpenXmlTemplate(
            IEnumerable<Run> begin,
            IEnumerable<OpenXmlElement> elements,
            IEnumerable<Run> after)
        {
            this.Begin = begin.ToList();
            this.Elements = elements.ToList();
            this.After = after.ToList();
        }

        public IReadOnlyList<Run> Begin { get; }
        public IReadOnlyList<OpenXmlElement> Elements { get; }
        public IReadOnlyList<Run> After { get; }

        public Body CreateBody()
        {
            return new Body(this.Elements.Select(e => e.CloneNode(true)));
        }
    }
}
