using Docx.DataModel;
using Xunit;

namespace Docx.Tests
{
    public class EngineConfigTests : TestBase
    {
        public EngineConfigTests() : base("EngineConfig")
        {
        }

        [Fact]
        public void DoubleQuote()
        {
            var config = new EngineConfig(
                new PlaceholderConfig("{{", "}}", ".", ":"),
                ArrayConfig.Default,
                ConditionConfig.Default);

            this.Process(nameof(DoubleQuote), new SimpleModel("xyz", "The real value of XYZ"), config);
        }
    }
}
