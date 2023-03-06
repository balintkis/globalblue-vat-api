namespace GlobalBlueVATApi.Service.Austria
{
    public interface IAustrianVatRateValidator
    {
        bool IsValid(decimal rateToCheck);
    }
}
