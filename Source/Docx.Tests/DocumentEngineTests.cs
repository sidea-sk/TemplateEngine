using Docx.DataModel;
using Xunit;

namespace Docx.Tests
{
    public class DocumentEngineTests : TestBase
    {
        public DocumentEngineTests() : base("DocumentEngine")
        {
        }

        [Fact]
        public void HelloWorld()
        {
            this.Process(nameof(HelloWorld), Model.Empty);
        }

        [Fact]
        public void AdHoc()
        {
            var model = new ObjectModel("p",
                new SimpleModel("nazov", "abcd"),
                new ObjectModel("adresa",
                    new SimpleModel("ulica", "address.Street"),
                    new SimpleModel("cislo", "address.StreetNumber"),
                    new SimpleModel("mesto", "address.City"),
                    new SimpleModel("stat", "address.Country"),
                    new SimpleModel("psc", "address.Zip")
                ));

            this.Process("Template", model);
        }
    }
}
