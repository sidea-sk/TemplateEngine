using Docx.DataModel;

namespace Docx.Serialization
{
    public static class Serializer
    {
        public static string Serialize(Model root)
        {
            var json = root.ToJson();
            return "{" + json + "}";
        }

        public static Model Deserialize(string json)
        {
            return Model.Empty;
        }
    }
}
