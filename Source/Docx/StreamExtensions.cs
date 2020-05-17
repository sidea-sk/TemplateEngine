using System.IO;

namespace Docx
{
    public static class StreamExtensions
    {
        public static MemoryStream AsMemoryStream(this byte[] data)
        {
            return new MemoryStream(data);
        }

        public static Stream AsStream(this byte[] data)
            => data.AsMemoryStream();
    }
}
