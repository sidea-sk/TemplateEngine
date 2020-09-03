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
        public void SimpleModelAsTheOnlyText()
        {
            this.Process(nameof(SimpleModelAsTheOnlyText), new SimpleModel("xyz", "The real value of XYZ"));
        }

        [Fact]
        public void SimpleModel()
        {
            this.Process(nameof(SimpleModel), new SimpleModel("xyz", "The real value of XYZ"));
        }

        [Fact]
        public void SimpleModelReturningNull()
        {
            this.Process(nameof(SimpleModelReturningNull), new SimpleModel("xyz", () => null));
        }

        [Fact]
        public void RepeatedSimpleModel()
        {
            this.Process(nameof(RepeatedSimpleModel), new SimpleModel("xyz", "XYZ for repetition"));
        }

        [Fact]
        public void SimpleModelMultipleParagraphs()
        {
            this.Process(nameof(SimpleModelMultipleParagraphs), new SimpleModel("xyz", "The replacement value!"));
        }

        [Fact]
        public void SimpleModelStyling()
        {
            this.Process(nameof(SimpleModelStyling), new SimpleModel("xyz", "The real value of XYZ"));
        }

        [Fact]
        public void SimpleModelInconsistentStyling()
        {
            this.Process(nameof(SimpleModelInconsistentStyling), new SimpleModel("xyz", "The real value of XYZ"));
        }

        [Fact]
        public void DuplicatedCharsSimpleValues()
        {
            this.Process(nameof(DuplicatedCharsSimpleValues), new SimpleModel("xyz", "The real value of XYZ"));
        }

        [Fact]
        public void MultipleSimpleModels()
        {
            this.Process(nameof(MultipleSimpleModels), new SimpleModel("xyz", "The real value of XYZ"));
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
        public void ObjectModelOmittedRootName()
        {
            var @object = new ObjectModel(
                "",
                new SimpleModel("a", "A - value"),
                new SimpleModel("b", "B - value"),
                new SimpleModel("c", "C - value")
                );

            this.Process(nameof(ObjectModelOmittedRootName), @object);
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

        [Fact(Skip = "not implemented")]
        public void CollectionModelInSameParagraph()
        {
            var items = Enumerable.Range(0, 5)
                .Select(i => new SimpleModel("$i", () => i.ToString()));

            var model = new CollectionModel(
                "root",
                items,
                new Model[0]);

            this.Process(nameof(CollectionModelInSameParagraph), model);
        }

        [Fact]
        public void CollectionModelWithTableInParagraphs()
        {
            var items = Enumerable.Range(0, 5)
                .Select(i => new SimpleModel("$i", () => i.ToString()));

            var model = new CollectionModel(
                "root",
                items,
                new Model[0]);

            this.Process(nameof(CollectionModelWithTableInParagraphs), model);
        }

        [Fact]
        public void CollectionOfObjectModel()
        {
            var items = Enumerable.Range(0, 5)
                .Select(i => new ObjectModel("$i", new SimpleModel("value", () => i.ToString())));

            var model = new CollectionModel(
                "collection",
                items,
                new Model[0]);

            this.Process(nameof(CollectionOfObjectModel), model);
        }

        [Fact]
        public void CollectionModelParagraphs()
        {
            var items = Enumerable.Range(0, 5)
                .Select(i => new SimpleModel("$i", () => i.ToString()));

            var model = new CollectionModel(
                "root",
                items,
                new Model[0]);

            this.Process(nameof(CollectionModelParagraphs), model);
        }

        [Fact]
        public void ConditionModel()
        {
            var model = new ObjectModel("",
                new ConditionModel("trueCondition", true),
                new ConditionModel("falseCondition", false)
            );

            this.Process(nameof(ConditionModel), model);
        }

        [Fact]
        public void ConditionModelInOneLine()
        {
            var model = new ObjectModel("",
                new ConditionModel("isTrue", true),
                new ConditionModel("isFalse", false)
            );

            this.Process(nameof(ConditionModelInOneLine), model);
        }
    }
}
