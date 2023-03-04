using FluentAssertions;
using GlobalBlueVATApi.Controllers;
using GlobalBlueVATApi.Model;
using GlobalBlueVATApi.Service;
using GlobalBlueVATApi.Views;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace UnitTestsGlobalBlueVatApi.Controllers
{
    public class PriceDetailsControllerTests
    {

        private Mock<IService> _service;
        private Mock<ILogger<PriceDetailsController>> _logger;
        private PriceDetailsController _controller;

        [SetUp]
        public void Setup()
        {
            _service = new Mock<IService>();
            _logger = new Mock<ILogger<PriceDetailsController>>();
            _controller = new PriceDetailsController(_service.Object, _logger.Object);
        }

        [Test]
        public void GetPriceDetailsAustriaTestValidRequest()
        {
            //Arrange
            _service.Setup(x => x.VerifyAustrianPriceDetailsInput(It.IsAny<PriceDetailsData>())).Returns(true);
            var expectedResult = "Test JSON";
            _service.Setup(x => x.CalculateAustrianNetGrossVatAmounts(It.IsAny<PriceDetailsData>())).Returns(expectedResult);
            var input = new PriceDetailsInput(100.0m, 0.0m, 0.0m, 0.2m);

            //Act
            var result = _controller.GetPriceDetailsAustria(input);

            //Assert
            _service.Verify(x => x.VerifyAustrianPriceDetailsInput(It.IsAny<PriceDetailsData>()), Times.Once);
            _service.Verify(x => x.CalculateAustrianNetGrossVatAmounts(It.IsAny<PriceDetailsData>()), Times.Once);

            _logger.Verify(
                m => m.Log(
                    LogLevel.Information,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, _) => v.ToString().Contains("Executing")),
                    null,
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()),
                    Times.Exactly(2));

            result.Should().NotBeNull();
            result.Should().BeOfType<OkObjectResult>();
            ((OkObjectResult)result).StatusCode.Should().Be(200);
            ((OkObjectResult)result).Value.Should().Be(expectedResult);
        }

        [Test]
        public void GetPriceDetailsAustriaTestInValidRequest()
        {
            //Arrange
            var expectedErrorMessage = "Wrong input";
            _service.Setup(x => x.VerifyAustrianPriceDetailsInput(It.IsAny<PriceDetailsData>())).Throws(new ArgumentException(expectedErrorMessage));
            
            var input = new PriceDetailsInput(100.0m, 100.0m, 0.0m, 0.2m);

            //Act
            var result = _controller.GetPriceDetailsAustria(input);

            //Assert
            _service.Verify(x => x.VerifyAustrianPriceDetailsInput(It.IsAny<PriceDetailsData>()), Times.Once);
            _service.Verify(x => x.CalculateAustrianNetGrossVatAmounts(It.IsAny<PriceDetailsData>()), Times.Never);

            _logger.Verify(
                m => m.Log(
                    LogLevel.Information,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, _) => v.ToString().Contains("Bad Request")),
                    null,
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()),
                    Times.Once());

            result.Should().NotBeNull();
            result.Should().BeOfType<BadRequestObjectResult>();
            ((BadRequestObjectResult)result).StatusCode.Should().Be(400);
            ((BadRequestObjectResult)result).Value.Should().Be(expectedErrorMessage);
        }

        /* 
         * As for passing non-decimal values: the .NET model binding
         * catches those: and returns 400 with a meaningful error message
         */
    }
}
