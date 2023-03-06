using GlobalBlueVATApi.Repository;

namespace GlobalBlueVATApi.Service.Austria
{
    public class AustrianVatRateValidator : IAustrianVatRateValidator
    {
        /* This singleton would serve as the in memory-cache for the valid VAT rates
         * probably stored in SQL in a real environment and kept up to date
         * via something like SqlTableDependency */

        private readonly IRepository _repository;
        public List<decimal> ValidRates { get; private set; }

        public AustrianVatRateValidator(IRepository repository)
        {
            _repository = repository;
            ValidRates = _repository.GetAustrianVATRates();
        }

        public bool IsValid(decimal rateToCheck)
        {
            return ValidRates.Any(x => ValidRates.Contains(rateToCheck));
        }
    }
}
