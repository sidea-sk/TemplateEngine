using DocumentFormat.OpenXml.Packaging;

namespace Docx.Processors.Images
{
    internal static class Tools
    {
        public static ImagePartType ImagePartTypeFromName(this string imageName)
        {
            var index = imageName?.LastIndexOf('.') ?? -1;
            if (index == -1)
            {
                return ImagePartType.Bmp;
            }

            var extension = imageName.Substring(index).ToLower();
            return extension switch
            { 
                "png" => ImagePartType.Png,
                _ => ImagePartType.Bmp
            };
        }
    }
}
