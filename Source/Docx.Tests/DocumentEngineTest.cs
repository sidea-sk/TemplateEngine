using Xunit;

namespace Docx.Tests
{
    public class DocumentEngineTest : TestBase
    {
        public DocumentEngineTest() : base("DocumentEngine")
        {
        }

        [Fact]
        public void HelloWorld()
        {
            this.Process(nameof(HelloWorld));
        }
    }
}
