using System.Linq;

namespace Docx
{
    internal static class StringExtensions
    {
        public static int IndexOfAny(this string text, params string[] values)
            => text.IndexOfAny(0, values);

        public static int IndexOfAny(this string text, int startIndex, params string[] values)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                return -1;
            }

            var minIndex = values
                .Select(v => text.IndexOf(v, startIndex))
                .Where(i => i >= 0)
                .MinOrDefault(-1);

            return minIndex;
        }
    }
}
