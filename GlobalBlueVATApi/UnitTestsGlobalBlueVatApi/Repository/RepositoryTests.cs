using FluentAssertions;

namespace UnitTestsGlobalBlueVatApi.Repository
{
    public class RepositoryTests
    {
        private GlobalBlueVATApi.Repository.IRepository _repository;

        [SetUp]
        public void Setup()
        {
            _repository = new GlobalBlueVATApi.Repository.Repository();
        }

        [Test]
        public void GetAustrianVATRatesTest()
        {
            //Arrange
            /* Well this is where we would setup MOQ to mock out the actual connection to the database and to return
             * a hardcoded VATRate list instead for testing purposes to see if the repository layer mapped it correctly
             * to the type we expect to get back from it, but our repository implentation is already using hardcoded values*/
            
            //Act
            var result = _repository.GetAustrianVATRates();

            //Assert
            result.Should().BeOfType<List<decimal>>();
            result.Should().NotBeNullOrEmpty();
        }
    }
}
