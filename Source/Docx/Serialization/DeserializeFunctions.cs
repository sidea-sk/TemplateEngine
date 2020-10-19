using System;
using System.Collections.Generic;
using System.Linq;
using Docx.DataModel;
using Newtonsoft.Json.Linq;

namespace Docx.Serialization
{
    internal static class DeserializeFunctions
    {
        public static Model ToModel(this JProperty jProperty)
        {
            var model = ((JObject)jProperty.Value).ToModel(jProperty.Name);
            return model;
        }

        public static Model ToModel(this JObject jToken, string modelName)
        {
            var type = jToken.GetValueOfProperty(Constants.TypeProperty);

            switch (type)
            {
                case nameof(CollectionModel):
                    {
                        var childs = jToken.GetChildModels();
                        var items = jToken.GetItemsModels();
                        return new CollectionModel(modelName, items, childs);
                    }
                case nameof(ObjectModel):
                    {
                        var childs = jToken.GetChildModels();
                        return new ObjectModel(modelName, childs);
                    }
                case nameof(SimpleModel):
                    var value = jToken.GetValueOfProperty(Constants.ValueProperty);
                    return new SimpleModel(modelName, value);
                case nameof(ConditionModel):
                    var condition = Convert.ToBoolean(jToken.GetValueOfProperty(Constants.ValueProperty));
                    return new ConditionModel(modelName, condition);
                case nameof(ImageModel):
                    var data = Convert.FromBase64String(jToken.GetValueOfProperty(Constants.ValueProperty));
                    var imageName = jToken.GetValueOfProperty(Constants.ImageNameProperty);
                    return new ImageModel(modelName, imageName, data);
            }

            return Model.Empty;
        }

        private static IEnumerable<Model> GetChildModels(this JObject parent)
        {
            var reserved = new[] { Constants.TypeProperty, Constants.ItemsProperty, Constants.ItemNameProperty };

            var children = parent
                .Children<JProperty>()
                .Where(p => !reserved.Contains(p.Name) && p.Name != Constants.RootNameProperty)
                .Select(p => p.ToModel())
                .ToArray();

            return children;
        }

        private static IEnumerable<Model> GetItemsModels(this JObject parent)
        {
            var itemName = parent.GetValueOfProperty(Constants.ItemNameProperty);
            var itemsProperty = parent
                .Children<JProperty>()
                .Single(p => p.Name == Constants.ItemsProperty);

            var itemArray = (itemsProperty.Value as JArray)
                .OfType<JObject>()
                .Select(i => i.ToModel(itemName))
                .ToArray()
                ;

            return itemArray;
        }

        private static string GetValueOfProperty(this JProperty property, string propertyName)
            => ((JObject)property.Value).GetValueOfProperty(propertyName);

        private static string GetValueOfProperty(this JObject jObject, string propertyName)
        {
            var childProperty = jObject
                .Children<JProperty>()
                .Single(p => p.Name == propertyName);

            return childProperty.Value.ToString();
        }
    }
}
