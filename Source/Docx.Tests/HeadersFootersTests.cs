using Docx.DataModel;
using Xunit;

namespace Docx.Tests
{
    public class HeadersFootersTests : TestBase
    {
        public HeadersFootersTests() : base("HeadersFooters")
        {
        }

        [Fact]
        public void SimpleHeader()
        {
            this.Process(nameof(SimpleHeader), new SimpleModel("xyz", "The real value of XYZ"));
        }

        [Fact]
        public void SimpleFooter()
        {
            this.Process(nameof(SimpleFooter), new SimpleModel("xyz", "The real value of XYZ"));
        }
    }
}
