using Docx.DataModel;
using Xunit;

namespace Docx.Tests
{
    public class JsonTests
    {
        private const string NONAME_JSON = "{\"$$_rootName\": \"\", \"$$_type\": \"ObjectModel\", \"simple\":{\"$$_type\": \"SimpleModel\", \"$$_value\": \"1\"}}";
        private const string JSON = "{\"$$_rootName\": \"root\", \"$$_type\": \"ObjectModel\", \"simple\":{\"$$_type\": \"SimpleModel\", \"$$_value\": \"1\"}, \"conditionTrue\":{\"$$_type\": \"ConditionModel\", \"$$_value\": \"true\"}, \"conditionFalse\":{\"$$_type\": \"ConditionModel\", \"$$_value\": \"false\"}, \"collection\":{\"$$_type\": \"CollectionModel\", \"$$_items\": [{\"$$_type\": \"SimpleModel\", \"$$_value\": \"1\"}, {\"$$_type\": \"SimpleModel\", \"$$_value\": \"2\"}, {\"$$_type\": \"SimpleModel\", \"$$_value\": \"3\"}], \"$$_itemName\": \"$c\"}, \"image\":{\"$$_type\": \"ImageModel\", \"$$_name\": \"image.png\", \"$$_value\": \"AQIDBA==\"}}";

        [Fact]
        public void Serialize()
        {
            var root = new ObjectModel(
                "root",
                new SimpleModel("simple", "1"),
                new ConditionModel("conditionTrue", () => true),
                new ConditionModel("conditionFalse", () => false),
                new CollectionModel(
                    "collection",
                    new Model[] {
                        new SimpleModel("$c", "1"),
                        new SimpleModel("$c", "2"),
                        new SimpleModel("$c", "3"),
                    },
                    new Model[0]
                ),
                new ImageModel("image", "image.png", new byte[] {1 ,2, 3, 4})
            );

            var json = Serialization.Serializer.Serialize(root);

            Assert.Equal(
                JSON,
                json);
        }

        [Fact]
        public void SerializeModelWithoutName()
        {
            var root = new ObjectModel(
                "",
                new SimpleModel("simple", "1")
            );

            var json = Serialization.Serializer.Serialize(root);

            Assert.Equal(
                NONAME_JSON,
                json);
        }

        [Fact]
        public void Deserialize()
        {
            var root = Serialization.Serializer.Deserialize(JSON) as ObjectModel;
            Assert.NotNull(root);
        }
    }
}
