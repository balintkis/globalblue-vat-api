using GlobalBlueVATApi.Service;
using GlobalBlueVATApi.Views;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace GlobalBlueVATApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PriceDetailsController : ControllerBase
    {
        private readonly IService _service;
        private readonly ILogger _logger;

        public PriceDetailsController(IService service, ILogger<PriceDetailsController> logger)
        {
            _service = service;
            _logger = logger;
        }

        // GET: api/GetPriceDetailsAustria
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpGet]
        public IActionResult GetPriceDetailsAustria([FromQuery]PriceDetailsInput priceDetailsInput)
        {
            _logger.LogInformation($"Executing {nameof(GetPriceDetailsAustria)} at {DateTime.UtcNow} with parameters: {JsonConvert.SerializeObject(priceDetailsInput)}");
            var mappedPriceDetails = InputMapper.MapPriceDetailsData(priceDetailsInput);

            try
            {
                _service.VerifyAustrianPriceDetailsInput(mappedPriceDetails);
            }
            catch (ArgumentException ex)
            {
                _logger.LogInformation($"Executing {nameof(GetPriceDetailsAustria)} at {DateTime.UtcNow} with parameters: {JsonConvert.SerializeObject(priceDetailsInput)} resulted in Bad Request returned due to {ex.Message}");
                return BadRequest(ex.Message);
            }

            var response = _service.CalculateAustrianNetGrossVatAmounts(mappedPriceDetails);
            _logger.LogInformation($"Executing {nameof(GetPriceDetailsAustria)} at {DateTime.UtcNow} with parameters: {JsonConvert.SerializeObject(priceDetailsInput)} resulted in OK with {response}");
            return Ok(response);
        }
    }
}
