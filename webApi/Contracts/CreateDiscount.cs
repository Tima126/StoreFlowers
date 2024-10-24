namespace webApi.Contracts
{
    public class CreateDiscount
    {
       

        public string Code { get; set; } = null!;

        public decimal DiscountPercentage { get; set; }

        public DateOnly ExpiryDate { get; set; }
    }
}
