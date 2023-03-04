namespace GlobalBlueVATApi.Repository
{
    public class Repository : IRepository
    {
        public List<decimal> GetAustrianVATRates()
        {
            return new List<decimal> { 0.1m, 0.13m, 0.2m };
        }
    }
}
