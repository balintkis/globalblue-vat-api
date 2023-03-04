using GlobalBlueVATApi.Model;
using GlobalBlueVATApi.Views;
using FluentAssertions;

namespace UnitTestsGlobalBlueVatApi.Service
{
    public class InputMapperTests
    {
        [TestCase(100, 0, 0, 0.2)]
        [TestCase(100, 100, 100, 100)]
        [TestCase(72.9678, 0, 0, 0.2)]
        public void MapPriceDetailsDataTest(decimal netAmount, decimal grossAmount, decimal vatAmount, decimal vatRate)
        {
            //Arrange
            var priceDetailsInput = new PriceDetailsInput(netAmount, grossAmount, vatAmount, vatRate);

            //Act
            var result = GlobalBlueVATApi.Service.InputMapper.MapPriceDetailsData(priceDetailsInput);

            //Assert
            result.Should().BeOfType<PriceDetailsData>();
            result.Should().BeEquivalentTo(priceDetailsInput);
        }

        [TestCase(-100, 0, 0, -0.2)]
        [TestCase(0, -78541.11232130, 0, -0.2)]
        [TestCase(-10, -78541.11232130, -10, -0.2)]
        public void MapPriceDetailsNegativeDataTest(decimal netAmount, decimal grossAmount, decimal vatAmount, decimal vatRate)
        {
            //Arrange
            var priceDetailsInput = new PriceDetailsInput(netAmount, grossAmount, vatAmount, vatRate);

            //Act
            var result = GlobalBlueVATApi.Service.InputMapper.MapPriceDetailsData(priceDetailsInput);

            //Assert
            result.Should().BeOfType<PriceDetailsData>();
            result.NetAmount.Should().Be(Math.Abs(priceDetailsInput.NetAmount));
            result.GrossAmount.Should().Be(Math.Abs(priceDetailsInput.GrossAmount));
            result.VatAmount.Should().Be(Math.Abs(priceDetailsInput.VatAmount));
            result.VatRate.Should().Be(Math.Abs(priceDetailsInput.VatRate));
        }
    }
}
