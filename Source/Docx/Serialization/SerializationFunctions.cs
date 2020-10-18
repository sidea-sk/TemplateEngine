using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using DocumentFormat.OpenXml.Bibliography;
using DocumentFormat.OpenXml.Wordprocessing;
using Docx.DataModel;

namespace Docx.Serialization
{
    internal static class SerializationFunctions
    {
        public static string ToJson(this Model model, bool omitName = false)
        {
            var json = omitName
                ?
                "" :
                $"\"{model.Name}\":";

            var properties = string.Join(", ", JsonProperties(model));
            json += "{"
                + properties
                + "}";

            return json;
        }

        private static IEnumerable<string> JsonProperties(Model model)
        {
            const string tmp = "\"{0}\": {1}";

            var type = model.GetType();
            yield return string.Format(tmp, "$type", "\"" + type + "\"");

            switch (model)
            {
                case SimpleModel _:
                case ImageModel _:
                    yield return string.Format(tmp, "value", "\"" + model.FormattedValue()) + "\"";
                    break;
                case ConditionModel _:
                    yield return string.Format(tmp, "value", model.FormattedValue());
                    break;
                case ObjectModel om:
                    var js = om.Childs.ToJson(omitName: false);
                    yield return js;
                    break;
            }

            if(model is CollectionModel com)
            {
                var itemName = com.Items.Count == 0
                    ? string.Empty
                    : com.Items.ElementAt(0).Name;

                var items = com.Items.ToJson(omitName: true);
                yield return $"\"$items\": [{string.Join(", ", items)}]";
                yield return string.Format(tmp, "$itemName", "\"" + itemName + "\"");
            }
        }

        private static string ToJson(this IEnumerable<Model> models, bool omitName)
        {
            return string.Join(", ", models.Select(m => m.ToJson(omitName)).ToArray());
        }

        private static string ChildsToJson(this ObjectModel objectModel)
        {
            var childs = objectModel.Childs.Select(c => c.ToJson());
            return string.Join(", ", childs);
        }
    }
}
