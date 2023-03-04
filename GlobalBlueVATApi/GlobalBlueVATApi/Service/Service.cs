using GlobalBlueVATApi.Model;
using GlobalBlueVATApi.Service.Austria;

namespace GlobalBlueVATApi.Service
{
    public class Service : IService
    {
        private readonly IAustriaServiceHelper _austriaPriceDetailsService;

        public Service(IAustriaServiceHelper austriaPriceDetailsService)
        {
            _austriaPriceDetailsService = austriaPriceDetailsService;
        }

        public bool VerifyAustrianVATRates(decimal rate)
        {
            return _austriaPriceDetailsService.VerifyVATRates(rate);
        }

        public bool VerifyAustrianPriceDetailsInput(PriceDetailsData inputToVerify)
        {
            return _austriaPriceDetailsService.VerifyPriceDetailsInput(inputToVerify);
        }

        public string CalculateAustrianNetGrossVatAmounts(PriceDetailsData inputToVerify)
        {
            return _austriaPriceDetailsService.Calculate(inputToVerify);
        }
    }
}
