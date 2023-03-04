using FluentAssertions;
using Moq;
using GlobalBlueVATApi.Repository;
using GlobalBlueVATApi.Service.Austria;

namespace UnitTestsGlobalBlueVatApi.Service.Austria
{
    public class AustrianVATRateValidatorTests
    {
        private static readonly object[] _validTestScenarios =
        {
            new object[] {new List<decimal> {0.1m, 0.13m, 0.2m}, 0.1m},
            new object[] {new List<decimal> {0.1m, 0.2m, 0.3m}, 0.3m}
        };

        private static readonly object[] _invalidTestScenarios =
        {
            new object[] {new List<decimal> {0.1m, 0.13m, 0.2m}, 0.5m},
            new object[] {new List<decimal> {0.1m, 0.2m, 0.3m}, 0.33m},
            new object[] {new List<decimal> {0.1m, 0.2m, 0.3m}, 0.0m},
            new object[] {new List<decimal> {0.1m, 0.2m, 0.3m}, 60000.0m},
            new object[] {new List<decimal> {0.1m, 0.2m, 0.3m}, -1.0m}
        };

        [TestCaseSource(nameof(_validTestScenarios))]
        public void IsValidTests(List<decimal> vatRateList, decimal rateToCheck)
        {
            //Arrange
            /* We pretend that we have a real repository which needs to be mocked... */
            var mockRepository = new Mock<IRepository>();
            mockRepository.Setup(x => x.GetAustrianVATRates()).Returns(vatRateList);

            var austrianVATValidator = new AustrianVATRateValidator(mockRepository.Object);

            //Act
            var result = austrianVATValidator.IsValid(rateToCheck);

            //Assert
            result.Should().BeTrue();
        }

        [TestCaseSource(nameof(_invalidTestScenarios))]
        public void IsNotValidTests(List<decimal> vatRateList, decimal rateToCheck)
        {
            //Arrange
            var mockRepository = new Mock<IRepository>();
            mockRepository.Setup(x => x.GetAustrianVATRates()).Returns(vatRateList);

            var austrianVATValidator = new AustrianVATRateValidator(mockRepository.Object);

            //Act
            var result = austrianVATValidator.IsValid(rateToCheck);

            //Assert
            result.Should().BeFalse();
        }
    }
}
