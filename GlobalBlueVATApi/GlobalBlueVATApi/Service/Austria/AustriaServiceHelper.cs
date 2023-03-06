using GlobalBlueVATApi.Model;
using Newtonsoft.Json;

namespace GlobalBlueVATApi.Service.Austria
{
    public class AustriaServiceHelper : IAustriaServiceHelper
    {
        private readonly IAustrianVatRateValidator _vatRateValidator;

        public AustriaServiceHelper(IAustrianVatRateValidator austrianVATRateValidator)
        {
            _vatRateValidator = austrianVATRateValidator;
        }

        #region Validators
        public bool VerifyVATRates(decimal rate)
        {
            return _vatRateValidator.IsValid(rate);
        }

        public bool VerifyPriceDetailsInput(PriceDetailsData inputToVerify)
        {
            if (!VerifyVATRates(inputToVerify.VatRate))
                throw new ArgumentException("Missing or invalid VAT rate!");

            var validParameterList = new List<bool>
            {
                !(inputToVerify.NetAmount == 0),
                !(inputToVerify.GrossAmount == 0),
                !(inputToVerify.VatAmount == 0)
            };

            if (!validParameterList.Any(x => x == true))
                throw new ArgumentException("No valid input (Net or Gross or Vat amount)!");

            if (validParameterList.Count(x => x == true) >= 2)
                throw new ArgumentException("More than one input amount (Net or Gross or Vat)!");

            return true;
        }
        #endregion

        #region Calculators
        public string Calculate(PriceDetailsData input)
        {
            var output = new PriceDetailsData();
            output.VatRate = input.VatRate;

            if (input.NetAmount != 0)
            {
                output.NetAmount = input.NetAmount;
                output.GrossAmount = CalculateGross(output.NetAmount, output.VatRate);
                output.VatAmount = CalculateVat(output.NetAmount, output.VatRate);
            }

            if (input.GrossAmount != 0)
            {
                output.GrossAmount = input.GrossAmount;
                output.NetAmount = CalculateNetFromGross(output.GrossAmount, output.VatRate);
                output.VatAmount = CalculateVat(output.NetAmount, output.VatRate);
            }

            if (input.VatAmount != 0)
            {
                output.VatAmount = input.VatAmount;
                output.NetAmount = CalculateNetFromVat(output.VatAmount, output.VatRate);
                output.GrossAmount = CalculateGross(output.NetAmount, input.VatRate);
            }

            return JsonConvert.SerializeObject(output);
        }

        public decimal CalculateNetFromGross(decimal grossAmount, decimal vatRate)
        {
            return grossAmount / (1.0m + vatRate);
        }

        public decimal CalculateNetFromVat(decimal vatAmount, decimal vatRate)
        {
            return vatAmount / vatRate;
        }

        public decimal CalculateGross(decimal netAmount, decimal vatRate)
        {
            return netAmount + (netAmount * vatRate);
        }

        public decimal CalculateVat(decimal netAmount, decimal vatRate)
        {
            return netAmount * vatRate;
        }
        #endregion
    }
}
