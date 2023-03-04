namespace GlobalBlueVATApi.Service.Austria
{
    public interface IAustrianVATRateValidator
    {
        bool IsValid(decimal rateToCheck);
    }
}