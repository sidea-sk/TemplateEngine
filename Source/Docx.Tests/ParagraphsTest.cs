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
        public void SimpleValueAsTheOnlyText()
        {
            this.Process(nameof(SimpleValueAsTheOnlyText), new SimpleModel("xyz", "The real value of XYZ"));
        }

        [Fact]
        public void SimpleValue()
        {
            this.Process(nameof(SimpleValue), new SimpleModel("xyz", "The real value of XYZ"));
        }

        [Fact]
        public void RepeatedSimpleValue()
        {
            this.Process(nameof(RepeatedSimpleValue), new SimpleModel("xyz", "XYZ for repetition"));
        }

        [Fact]
        public void SimpleValueStyling()
        {
            this.Process(nameof(SimpleValueStyling), new SimpleModel("xyz", "The real value of XYZ"));
        }

        [Fact]
        public void SimpleValueInconsistentStyling()
        {
            this.Process(nameof(SimpleValueInconsistentStyling), new SimpleModel("xyz", "The real value of XYZ"));
        }

        [Fact]
        public void DuplicatedCharsSimpleValues()
        {
            this.Process(nameof(DuplicatedCharsSimpleValues), new SimpleModel("xyz", "The real value of XYZ"));
        }

        [Fact]
        public void MultipleSimpleValues()
        {
            this.Process(nameof(MultipleSimpleValues), new SimpleModel("xyz", "The real value of XYZ"));
        }
    }
}
