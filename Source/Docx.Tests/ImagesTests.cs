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

        private ImageModel LoadFromFile(string modelName, string imageName)
        {
            var data = File.ReadAllBytes(this.SamplesFolder + "/" + imageName);
            return new ImageModel(modelName, imageName, data);
        }
    }
}
