namespace GlobalBlueVATApi.Views
{
    public class PriceDetailsInput
    {
        public decimal NetAmount { get; set; }
        public decimal GrossAmount { get; set; }
        public decimal VatAmount { get; set; }
        public decimal VatRate { get; set; }

        public PriceDetailsInput(decimal netAmount, decimal grossAmount, decimal vatAmount, decimal vatRate)
        {
            NetAmount = netAmount;
            GrossAmount = grossAmount;
            VatAmount = vatAmount;
            VatRate = vatRate;
        }

        public PriceDetailsInput()
        {
        }
    }
}
