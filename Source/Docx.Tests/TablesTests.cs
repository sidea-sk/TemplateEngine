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
    }
}
