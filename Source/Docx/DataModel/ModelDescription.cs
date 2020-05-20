using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Docx.DataModel
{
    [DebuggerDisplay("{DebugString()}")]
    internal class ModelDescription
    {
        public static readonly ModelDescription Empty = new ModelDescription(new string[0], string.Empty, string.Empty);

        private readonly string[] _segments;
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
            _segments = segments.ToArray();
            _parameters = parameters;

            OriginalText = originalText;
        }

        // path
        public string Name => _segments.LastOrDefault() ?? string.Empty;

        public string OriginalText { get; }

        private string DebugString()
        {
            return string.Join(".", _segments);
        }
    }
}
