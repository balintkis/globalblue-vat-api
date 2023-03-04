using GlobalBlueVATApi.Model;

namespace GlobalBlueVATApi.Service
{
    public interface IService
    {
        string CalculateAustrianNetGrossVatAmounts(PriceDetailsData inputToVerify);
        bool VerifyAustrianPriceDetailsInput(PriceDetailsData inputToVerify);
        bool VerifyAustrianVATRates(decimal rate);
    }
}
