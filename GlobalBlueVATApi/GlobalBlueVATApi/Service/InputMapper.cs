using GlobalBlueVATApi.Model;
using GlobalBlueVATApi.Views;

namespace GlobalBlueVATApi.Service
{
    public static class InputMapper
    {
        public static PriceDetailsData MapPriceDetailsData(PriceDetailsInput input) 
        {
            return new PriceDetailsData(input.NetAmount, input.GrossAmount, input.VatAmount, input.VatRate);
        }
    }
}
