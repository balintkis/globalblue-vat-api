using GlobalBlueVATApi.Model;

namespace GlobalBlueVATApi.Service.Austria
{
    public interface IAustriaServiceHelper
    {
        bool VerifyPriceDetailsInput(PriceDetailsData inputToVerify);
        bool VerifyVATRates(decimal rate);
        string Calculate(PriceDetailsData input);
    }
}