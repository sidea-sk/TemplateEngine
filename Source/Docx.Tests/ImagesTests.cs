using System.IO;
using Docx.DataModel;
using Xunit;

namespace Docx.Tests
{
    public class ImagesTests : TestBase
    {
        public ImagesTests() : base("Images")
        {
        }

        [Fact]
        public void Image()
        {
            var model = this.LoadFromFile("image", "sample.jpeg");

            this.Process(nameof(Image), model);
        }

        [Fact]
        public void ImageInTheMiddleOfRun()
        {
            var model = this.LoadFromFile("image", "sample.jpeg");

            this.Process(nameof(ImageInTheMiddleOfRun), model);
        }

        [Fact]
        public void ImageSizeParameters()
        {
            var model = this.LoadFromFile("image", "sample.jpeg");

            this.Process(nameof(ImageSizeParameters), model);
        }

        [Fact]
        public void ImageSizeParametersWidth()
        {
            var model = this.LoadFromFile("image", "sample.jpeg");

            this.Process(nameof(ImageSizeParametersWidth), model);
        }

        [Fact]
        public void ImageSizeParametersHeight()
        {
            var model = this.LoadFromFile("image", "sample.jpeg");

            this.Process(nameof(ImageSizeParametersHeight), model);
        }

        [Fact]
        public void ImageSizeParametersInch()
        {
            var model = this.LoadFromFile("image", "sample.jpeg");

            this.Process(nameof(ImageSizeParametersInch), model);
        }

        [Fact]
        public void EmptyImage()
        {
            var model = new ImageModel("image", "empty.jpeg", new byte[0]);
            this.Process(nameof(EmptyImage), model);
        }

        private ImageModel LoadFromFile(string modelName, string imageName)
        {
            var data = File.ReadAllBytes(SamplesFolder + "/" + imageName);
            return new ImageModel(modelName, imageName, data);
        }
    }
}
