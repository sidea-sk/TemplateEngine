using System;
using System.Collections.Generic;
using System.Text;
using Docx.DataModel;
using Xunit;

namespace Docx.Tests
{
    public class JsonTests
    {
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
                        new SimpleModel("$c", "a")
                    },
                    new Model[0]
                )
            );

            // var json = root.SerializeToJson();
        }
    }
}
