using Docx.DataModel;
using Xunit;

namespace Docx.Tests
{
    public class ParagraphsTest : TestBase
    {
        public ParagraphsTest() : base("Paragraphs")
        {
        }

        [Fact]
        public void SimpleValue()
        {
            this.Process(nameof(SimpleValue), new SimpleModel("xyz", "Replaced value of XYZ"));
        }
    }
}
