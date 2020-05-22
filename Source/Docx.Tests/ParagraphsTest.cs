using System.Linq;
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
        public void SimpleValueMultipleParagraphs()
        {
            this.Process(nameof(SimpleValueMultipleParagraphs), new SimpleModel("xyz", "The replacement value!"));
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

        [Fact]
        public void ObjectModel()
        {
            var @object = new ObjectModel(
                "object",
                new SimpleModel("a", "A - value"),
                new SimpleModel("b", "B - value"),
                new SimpleModel("c", "C - value")
                );

            this.Process(nameof(ObjectModel), @object);
        }

        [Fact]
        public void ObjectModelMultipleValues()
        {
            var @object = new ObjectModel(
                "object",
                new SimpleModel("a", "A - value"),
                new SimpleModel("b", "B - value"),
                new SimpleModel("c", "C - value")
                );

            this.Process(nameof(ObjectModelMultipleValues), @object);
        }

        [Fact]
        public void NestedObjectModel()
        {
            var level3 = new ObjectModel("level3", new SimpleModel("vl3", "value in level3"));
            var level2 = new ObjectModel("level2", level3);
            var level1 = new ObjectModel("level1", level2);

            this.Process(nameof(NestedObjectModel), level1);
        }

        [Fact]
        public void NestedObjectModelMultipleParagraphs()
        {
            var level3 = new ObjectModel("level3", new SimpleModel("vl3", "value in level3"));
            var level2 = new ObjectModel("level2", level3, new SimpleModel("vl2", "value in level2"));
            var level1 = new ObjectModel("level1", level2, new SimpleModel("vl1", "value in level1"));

            this.Process(nameof(NestedObjectModelMultipleParagraphs), level1);
        }

        [Fact]
        public void CollectionModel()
        {
            var items = Enumerable.Range(0, 5)
                .Select(i => new SimpleModel("$i", () => i.ToString()));

            var model = new CollectionModel(
                "root",
                items,
                new Model[0]);

            this.Process(nameof(CollectionModel), model);
        }
    }
}
