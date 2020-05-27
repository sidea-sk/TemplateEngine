using DocumentFormat.OpenXml.Wordprocessing;
using Docx.DataModel;

namespace Docx.Processors
{
    internal interface IImageProcessor
    {
        Run AddImage(ImageModel model);
    }
}
