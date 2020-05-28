using System.Drawing;
using System.IO;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using Docx.DataModel;
using Docx.Processors.Images;

using A = DocumentFormat.OpenXml.Drawing;
using DW = DocumentFormat.OpenXml.Drawing.Wordprocessing;
using PIC = DocumentFormat.OpenXml.Drawing.Pictures;

namespace Docx.Processors
{
    internal class ImageProcessor : IImageProcessor
    {
        private readonly MainDocumentPart _mainDocumentPart;

        public ImageProcessor(MainDocumentPart mainDocumentPart)
        {
            _mainDocumentPart = mainDocumentPart;
        }

        public Run AddImage(ImageModel model)
        {
            var imagePartType = model.ImageName.ImagePartTypeFromName();
            var imagePart = _mainDocumentPart.AddImagePart(imagePartType);

            using(var ms = new MemoryStream(model.Data))
            {
                imagePart.FeedData(ms);
            }

            var (width, height) = this.GetImageSizeInEmu(model.Data);
            var run = this.CreateRun(model.ImageName, _mainDocumentPart.GetIdOfPart(imagePart), width, height);
            return run;
        }

        private Run CreateRun(string imageName, string relationshipId, long imageWidth, long imageHeight)
        {
            var element = new Drawing(
             new DW.Inline(
                 new DW.Extent() { Cx = imageWidth, Cy = imageHeight },
                 new DW.EffectExtent()
                 {
                     LeftEdge = 0L,
                     TopEdge = 0L,
                     RightEdge = 0L,
                     BottomEdge = 0L
                 },
                 new DW.DocProperties()
                 {
                     Id = (UInt32Value)1U,
                     Name = "Picture"
                 },
                 new DW.NonVisualGraphicFrameDrawingProperties(
                     new A.GraphicFrameLocks() { NoChangeAspect = true }),
                 new A.Graphic(
                     new A.GraphicData(
                         new PIC.Picture(
                             new PIC.NonVisualPictureProperties(
                                 new PIC.NonVisualDrawingProperties()
                                 {
                                     Id = 0U,
                                     Name = imageName
                                 },
                                 new PIC.NonVisualPictureDrawingProperties()),
                             new PIC.BlipFill(
                                 new A.Blip(
                                     new A.BlipExtensionList(
                                         new A.BlipExtension()
                                         {
                                             Uri = "{28A0092B-C50C-407E-A947-70E740481C1C}"
                                         })
                                 )
                                 {
                                     Embed = relationshipId,
                                     CompressionState =
                                     A.BlipCompressionValues.Print
                                 },
                                 new A.Stretch(
                                     new A.FillRectangle())),
                             new PIC.ShapeProperties(
                                 new A.Transform2D(
                                     new A.Offset() { X = 0L, Y = 0L },
                                     new A.Extents() { Cx = imageWidth, Cy = imageHeight }),
                                 new A.PresetGeometry(
                                     new A.AdjustValueList()
                                 )
                                 { Preset = A.ShapeTypeValues.Rectangle }))
                     )
                     { Uri = "http://schemas.openxmlformats.org/drawingml/2006/picture" })
             )
             {
                 DistanceFromTop = 0U,
                 DistanceFromBottom = 0U,
                 DistanceFromLeft = 0U,
                 DistanceFromRight = 0U,
                 EditId = "50D07946"
             });

            var run = new Run(element);
            return run;
        }

        private (long width, long height) GetImageSizeInEmu(byte[] data)
        {
            using(var ms = new MemoryStream(data))
            {
                var image = Image.FromStream(ms);
                return (image.Width * 9525, image.Height * 9525);
            }
        }
    }
}
