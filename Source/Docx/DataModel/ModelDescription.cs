using System.Collections.Generic;
using System.Diagnostics;

namespace Docx.DataModel
{
    [DebuggerDisplay("{Expression}")]
    internal class ModelDescription
    {
        public static readonly ModelDescription Empty = new ModelDescription(new string[0], string.Empty, string.Empty);

        public ModelDescription(
            IEnumerable<string> segments,
            string originalText) : this(segments, string.Empty, originalText)
        {
        }

        public ModelDescription(
            IEnumerable<string> segments,
            string parameters,
            string originalText)
        {
            this.Parameters = parameters;
            this.Expression = new ModelExpression(segments);
            this.OriginalText = originalText;
        }

        public ModelExpression Expression { get; }

        public string Parameters { get; }

        public string OriginalText { get; }
    }
}
