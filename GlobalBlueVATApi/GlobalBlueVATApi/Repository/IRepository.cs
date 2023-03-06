namespace GlobalBlueVATApi.Repository
{
    public interface IRepository
    {
        List<decimal> GetAustrianVATRates();
    }
}
