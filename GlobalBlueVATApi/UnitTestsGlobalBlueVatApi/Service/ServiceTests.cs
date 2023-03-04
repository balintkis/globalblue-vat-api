using FluentAssertions;
using GlobalBlueVATApi.Model;
using GlobalBlueVATApi.Service;
using GlobalBlueVATApi.Service.Austria;
using Moq;

namespace UnitTestsGlobalBlueVatApi.Service
{
    public class ServiceTests
    {
        private Mock<IAustriaServiceHelper> _austriaPriceDetailsService;
        private IService _service;
        private string _resultJson = "Result JSON";

        [SetUp]
        public void Setup()
        {
            _austriaPriceDetailsService = new Mock<IAustriaServiceHelper>();
            _service = new GlobalBlueVATApi.Service.Service(_austriaPriceDetailsService.Object);

            _austriaPriceDetailsService.Setup(x => x.VerifyVATRates(It.IsAny<decimal>())).Returns(false);
            _austriaPriceDetailsService.Setup(x => x.VerifyPriceDetailsInput(It.IsAny<PriceDetailsData>())).Returns(false);
            _austriaPriceDetailsService.Setup(x => x.Calculate(It.IsAny<PriceDetailsData>())).Returns(_resultJson);
        }

        [Test]
        public void VerifyAustrianVATRatesTest()
        {
            //Arrange
            var inputRate = 0.13m;

            //Act
            var result = _service.VerifyAustrianVATRates(inputRate);

            //Assert
            _austriaPriceDetailsService.Verify(x => x.VerifyVATRates(inputRate), Times.Once);
            result.Should().BeFalse();
        }

        [Test]
        public void VerifyAustrianPriceDetailsInputTest()
        {
            //Arrange
            var inputData = new PriceDetailsData();

            //Act
            var result = _service.VerifyAustrianPriceDetailsInput(inputData);

            //Assert
            _austriaPriceDetailsService.Verify(x => x.VerifyPriceDetailsInput(inputData), Times.Once);
            result.Should().BeFalse();
        }

        [Test]
        public void CalculateAustrianNetGrossVatAmounts()
        {
            //Arrange
            var inputData = new PriceDetailsData();

            //Act
            var result = _service.CalculateAustrianNetGrossVatAmounts(inputData);

            //Assert
            _austriaPriceDetailsService.Verify(x => x.Calculate(inputData), Times.Once);
            result.Should().BeEquivalentTo(_resultJson);
        }
    }
}
