namespace GlobalBlueVATApi.Model
{
    public class PriceDetailsData
    {
        public decimal NetAmount { get; set; }
        public decimal GrossAmount { get; set; }
        public decimal VatAmount { get; set; }
        public decimal VatRate { get; set; }

        public PriceDetailsData(decimal netAmount, decimal grossAmount, decimal vatAmount, decimal vatRate)
        {
            NetAmount = Math.Abs(netAmount);
            GrossAmount = Math.Abs(grossAmount);
            VatAmount = Math.Abs(vatAmount);
            VatRate = Math.Abs(vatRate);
        }

        public PriceDetailsData()
        {
        }
    }

}
