using Docx.DataModel;
using Xunit;

namespace Docx.Tests
{
    public class JsonTests
    {
        private const string JSON = "{\"root\":{\"$type\": \"Docx.DataModel.ObjectModel\", \"simple\":{\"$type\": \"Docx.DataModel.SimpleModel\", \"value\": \"1\"}, \"conditionTrue\":{\"$type\": \"Docx.DataModel.ConditionModel\", \"value\": True}, \"conditionFalse\":{\"$type\": \"Docx.DataModel.ConditionModel\", \"value\": False}, \"collection\":{\"$type\": \"Docx.DataModel.CollectionModel\", , \"$items\": [{\"$type\": \"Docx.DataModel.SimpleModel\", \"value\": \"1\"}, {\"$type\": \"Docx.DataModel.SimpleModel\", \"value\": \"2\"}, {\"$type\": \"Docx.DataModel.SimpleModel\", \"value\": \"3\"}], \"$itemName\": \"$c\"}, \"image\":{\"$type\": \"Docx.DataModel.ImageModel\", \"value\": \"AQIDBA==\"}}}";
        
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
    }
}
