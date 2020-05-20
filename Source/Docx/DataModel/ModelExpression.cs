using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Docx.DataModel
{
    [DebuggerDisplay("{ToString()}")]
    internal class ModelExpression
    {
        private readonly string[] _segments;

        public ModelExpression(IEnumerable<string> segments)
        {
            _segments = segments.ToArray();
        }

        public string Root => _segments.FirstOrDefault() ?? string.Empty;
        public string Name => _segments.LastOrDefault() ?? string.Empty;

        public bool IsFinal => _segments.Length <= 1;

        public override string ToString()
        {
            return string.Join(".", _segments);
        }

        public ModelExpression Child()
        {
            return new ModelExpression(_segments.Skip(1));
        }
    }
}
