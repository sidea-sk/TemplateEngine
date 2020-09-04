using System;
using System.Collections.Generic;
using System.Text;
using Docx.DataModel;
using Xunit;

namespace Docx.Tests
{
    public class TablesTests : TestBase
    {
        public TablesTests(): base("Tables")
        {
        }

        [Fact]
        public void SimpleModel()
        {
            this.Process(nameof(SimpleModel), new SimpleModel("xyz", "The real value of XYZ"));
        }

        [Fact]
        public void CollectionModel()
        {
            var model = new CollectionModel("collection", new[] {
                new SimpleModel("$i", "1"),
                new SimpleModel("$i", "2"),
                new SimpleModel("$i", "3"),
                new SimpleModel("$i", "4"),
                new SimpleModel("$i", "5"),
            },
            new Model[0]);

            this.Process(nameof(CollectionModel), model);
        }

        [Fact]
        public void CollectionModelInSingleCell()
        {
            var model = new CollectionModel("collection", new[] {
                new SimpleModel("$i", "1"),
                new SimpleModel("$i", "2"),
                new SimpleModel("$i", "3"),
                new SimpleModel("$i", "4"),
                new SimpleModel("$i", "5"),
            },
            new Model[0]);

            this.Process(nameof(CollectionModelInSingleCell), model);
        }

        [Fact]
        public void CollectionModelTableWithNonTemplateRows()
        {
            var model = new CollectionModel("collection", new[] {
                new SimpleModel("$i", "1"),
                new SimpleModel("$i", "2"),
                new SimpleModel("$i", "3"),
                new SimpleModel("$i", "4"),
                new SimpleModel("$i", "5"),
            },
            new Model[0]);

            this.Process(nameof(CollectionModelTableWithNonTemplateRows), model);
        }

        [Fact]
        public void CollectionModelTableWithMixedTemplates()
        {
            var model = new ObjectModel(
                "root",
                new CollectionModel("collection", new[] {
                    new SimpleModel("$i", "1"),
                    new SimpleModel("$i", "2"),
                    new SimpleModel("$i", "3"),
                    new SimpleModel("$i", "4"),
                    new SimpleModel("$i", "5"),
                    },
                    new Model[0]),
                new SimpleModel("simpleValue", "the content of simple value"));

            this.Process(nameof(CollectionModelTableWithMixedTemplates), model);
        }
    }
}
