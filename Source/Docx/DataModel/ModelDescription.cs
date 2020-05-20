using System.Collections.Generic;
using System.Diagnostics;

namespace Docx.DataModel
{
    [DebuggerDisplay("{Expression}")]
    internal class ModelDescription
    {
        public static readonly ModelDescription Empty = new ModelDescription(new string[0], string.Empty, string.Empty);

        private readonly string _parameters;

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
            _parameters = parameters;
            this.Expression = new ModelExpression(segments);
            this.OriginalText = originalText;
        }

        public ModelExpression Expression { get; }

        public string OriginalText { get; }
    }
}
