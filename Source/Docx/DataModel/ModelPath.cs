using System.Collections.Generic;

namespace Docx.DataModel
{
    public class ModelPath
    {
        private readonly IEnumerable<string> _segments;

        public ModelPath(IEnumerable<string> segments)
        {
            _segments = segments;
        }
    }
}
