using GlobalBlueVATApi.Service.Austria;
using GlobalBlueVATApi.Model;
using FluentAssertions;
using Moq;
using Newtonsoft.Json;

namespace UnitTestsGlobalBlueVatApi.Service.Austria
{
    public class AustriaServiceHelperTests
    {
        private Mock<IAustrianVATRateValidator> _vatRateValidator;
        private AustriaServiceHelper _austriaServiceHelper;

        private static readonly object[] _verifyPriceDetailsValidTestScenarios =
        {
            new object[] {100.0m, 0.0m, 0.0m, 0.2m},
            new object[] {0.0m, 100.0m, 0.0m, 0.2m},
            new object[] {0.0m, 0.0m, 100.0m, 0.2m}
        };

        private static readonly object[] _verifyPriceDetailsInvalidTestScenarios =
        {
            new object[] {100.0m, 100.0m, 0.0m, 0.2m},
            new object[] {0.0m, 100.0m, 100.0m, 0.2m},
            new object[] {100.0m, 0.0m, 100.0m, 0.2m},
            new object[] {100.0m, 100.0m, 100.0m, 0.2m}
        };

        private static readonly object[] _calculateValidTestScenarios =
        {
            new object[] {new PriceDetailsData(100.0m, 0.0m, 0.0m, 0.2m), new PriceDetailsData(100.0m, 120.0m, 20.0m, 0.2m)},
            new object[] {new PriceDetailsData(100.0m, 0.0m, 0.0m, 0.1m), new PriceDetailsData(100.0m, 110.0m, 10.0m, 0.1m)},
            new object[] {new PriceDetailsData(100.0m, 0.0m, 0.0m, 0.13m), new PriceDetailsData(100.0m, 113.0m, 13.0m, 0.13m)},
            new object[] {new PriceDetailsData(0.0m, 120.0m, 0.0m, 0.2m), new PriceDetailsData(100.0m, 120.0m, 20.0m, 0.2m)},
            new object[] {new PriceDetailsData(0.0m, 0.0m, 20.0m, 0.2m), new PriceDetailsData(100.0m, 120.0m, 20.0m, 0.2m)}
        };

        [SetUp]
        public void Setup()
        {
            _vatRateValidator = new Mock<IAustrianVATRateValidator>();
            _austriaServiceHelper = new AustriaServiceHelper(_vatRateValidator.Object);
        }

        [Test]
        public void VerifyVATRatesTrueTest()
        {
            //Arrange
            _vatRateValidator.Setup(x => x.IsValid(It.IsAny<decimal>())).Returns(true);
            var inputRate = 0.13m;


            //Act
            var result = _austriaServiceHelper.VerifyVATRates(inputRate);

            //Assert
            _vatRateValidator.Verify(x => x.IsValid(inputRate), Times.Once);
            result.Should().BeTrue();
        }

        [Test]
        public void VerifyVATRatesFalseTest()
        {
            //Arrange
            _vatRateValidator.Setup(x => x.IsValid(It.IsAny<decimal>())).Returns(false);
            var inputRate = 0.13m;

            //Act
            var result = _austriaServiceHelper.VerifyVATRates(inputRate);

            //Assert
            _vatRateValidator.Verify(x => x.IsValid(inputRate), Times.Once);
            result.Should().BeFalse();
        }

        [TestCaseSource(nameof(_verifyPriceDetailsValidTestScenarios))]
        public void VerifyPriceDetailsInputTest(decimal net, decimal gros, decimal vat, decimal vatRate)
        {
            //Arrange
            _vatRateValidator.Setup(x => x.IsValid(It.IsAny<decimal>())).Returns(true);
            var input = new PriceDetailsData(net, gros, vat, vatRate);

            //Act
            var result = _austriaServiceHelper.VerifyPriceDetailsInput(input);

            //Assert
            result.Should().BeTrue();
        }

        [TestCaseSource(nameof(_verifyPriceDetailsInvalidTestScenarios))]
        public void VerifyPriceDetailsInputInvalidAsMoreThanOneInputTest(decimal net, decimal gros, decimal vat, decimal vatRate)
        {
            //Arrange
            var input = new PriceDetailsData(net, gros, vat, vatRate);
            _vatRateValidator.Setup(x => x.IsValid(It.IsAny<decimal>())).Returns(true);
            
            //Act
            var ex = Assert.Throws<ArgumentException>(() => _austriaServiceHelper.VerifyPriceDetailsInput(input));

           //Assert
            ex.Message.Should().BeEquivalentTo("More than one input amount (Net or Gross or Vat)!");          
        }

        [Test]
        public void VerifyPriceDetailsInputInvalidAsNoInputTest()
        {
            //Arrange
            var input = new PriceDetailsData(0.0m, 0.0m, 0.0m, 0.2m);
            _vatRateValidator.Setup(x => x.IsValid(It.IsAny<decimal>())).Returns(true);

            //Act
            var ex = Assert.Throws<ArgumentException>(() => _austriaServiceHelper.VerifyPriceDetailsInput(input));

            //Assert
            ex.Message.Should().BeEquivalentTo("No valid input (Net or Gross or Vat amount)!");
        }

        [Test]
        public void VerifyPriceDetailsInputInvalidAsBadVatTest()
        {
            //Arrange
            var input = new PriceDetailsData(100.0m, 0.0m, 0.0m, 0.0m);
            _vatRateValidator.Setup(x => x.IsValid(It.IsAny<decimal>())).Returns(false);

            //Act
            var ex = Assert.Throws<ArgumentException>(() => _austriaServiceHelper.VerifyPriceDetailsInput(input));

            //Assert
            ex.Message.Should().BeEquivalentTo("Missing or invalid VAT rate!");
        }

        [TestCaseSource(nameof(_calculateValidTestScenarios))]
        public void CalculateTest(PriceDetailsData inputPriceDetails, PriceDetailsData expectedResults)
        {
            //Arrange
            _vatRateValidator.Setup(x => x.IsValid(It.IsAny<decimal>())).Returns(true);

            //Act
            var result = _austriaServiceHelper.Calculate(inputPriceDetails);
            var resultDeserialized = JsonConvert.DeserializeObject<PriceDetailsData>(result);

            //Assert
            result.Should().BeOfType<string>();
            resultDeserialized.Should().BeEquivalentTo(expectedResults);
        }
    }
}
