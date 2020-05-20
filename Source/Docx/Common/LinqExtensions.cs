using System.Collections.Generic;
using System.Linq;

namespace Docx
{
    internal static class LinqExtensions
    {
        public static T MinOrDefault<T>(this IEnumerable<T> source, T ifEmpty)
        {
            var x = source.ToArray();
            if(x.Length == 0)
            {
                return ifEmpty;
            }

            return x.Min();
        }
    }
}
