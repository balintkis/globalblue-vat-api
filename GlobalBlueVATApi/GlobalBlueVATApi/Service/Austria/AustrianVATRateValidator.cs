using GlobalBlueVATApi.Repository;

namespace GlobalBlueVATApi.Service.Austria
{
    public class AustrianVATRateValidator : IAustrianVATRateValidator
    {
        /* This singleton would serve as the in memory-cache for the valid VAT rates
         * probably stored in SQL in a real environment and kept up to date
         * via something like SqlTableDependency */

        private readonly IRepository _repository;
        public List<decimal> _validRates { get; private set; }

        public AustrianVATRateValidator(IRepository repository)
        {
            _repository = repository;
            _validRates = _repository.GetAustrianVATRates();
        }

        public bool IsValid(decimal rateToCheck)
        {
            return _validRates.Any(x => _validRates.Contains(rateToCheck));
        }
    }
}
