using System.Collections.Generic;
using System.Linq;
using Docx.DataModel;

namespace Docx.Serialization
{
    internal static class SerializationFunctions
    {
        private const string StringPropertyTemplate = "\"{0}\": \"{1}\"";
        private const string ValuePropertyTemplate = "\"{0}\": \"{1}\"";

        public static string ToJson(this Model model, NameSerialization nameSerialization = NameSerialization.AsParent)
        {
            var json = nameSerialization == NameSerialization.AsParent
                ? $"\"{model.Name}\":"
                : string.Empty;

            var properties = JsonProperties(model).Where(js => !string.IsNullOrWhiteSpace(js)).ToList();
            if(nameSerialization == NameSerialization.AsProperty)
            {
                properties.Insert(0, string.Format(StringPropertyTemplate, Constants.RootNameProperty, model.Name));
            }

            json += "{"
                + string.Join(", ", properties)
                + "}";

            return json;
        }

        private static IEnumerable<string> JsonProperties(Model model)
        {
            var type = model.GetType().Name;
            yield return string.Format(StringPropertyTemplate, Constants.TypeProperty, type);

            switch (model)
            {
                case SimpleModel _:
                    yield return string.Format(StringPropertyTemplate, Constants.ValueProperty, model.FormattedValue());
                    break;
                case ImageModel im:
                    yield return string.Format(StringPropertyTemplate, Constants.ImageNameProperty, im.ImageName);
                    yield return string.Format(StringPropertyTemplate, Constants.ValueProperty, im.FormattedValue());
                    break;
                case ConditionModel _:
                    yield return string.Format(ValuePropertyTemplate, Constants.ValueProperty, model.FormattedValue());
                    break;
                case ObjectModel om:
                    var js = om.Childs.ToJson(NameSerialization.AsParent);
                    yield return js;
                    break;
            }

            if(model is CollectionModel com)
            {
                var itemName = com.Items.Count == 0
                    ? string.Empty
                    : com.Items.ElementAt(0).Name;

                var items = com.Items.ToJson(NameSerialization.None);
                yield return $"\"{Constants.ItemsProperty}\": [{string.Join(", ", items)}]";
                yield return string.Format(StringPropertyTemplate, Constants.ItemNameProperty, itemName);
            }
        }

        private static string ToJson(this IEnumerable<Model> models, NameSerialization nameSerialization)
        {
            return string.Join(", ", models.Select(m => m.ToJson(nameSerialization)).ToArray());
        }
    }
}
