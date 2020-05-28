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
                "jpg" => ImagePartType.Jpeg,
                "jpeg" => ImagePartType.Jpeg,
                "bmp" => ImagePartType.Bmp,
                "emf" => ImagePartType.Emf,
                "gif" => ImagePartType.Gif,
                "ico" => ImagePartType.Icon,
                "tiff" => ImagePartType.Tiff,
                "wmf" => ImagePartType.Wmf,
                "pcx" => ImagePartType.Pcx,
                _ => ImagePartType.Bmp
            };
        }
    }
}
