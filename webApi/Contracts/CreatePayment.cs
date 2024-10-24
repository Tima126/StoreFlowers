namespace webApi.Contracts
{
    public class CreatePayment
    {
       

        public int? OrderId { get; set; }

        public DateTime? PaymentDate { get; set; }

        public decimal Amount { get; set; }

        public int? PaymentMethodId { get; set; }
    }
}
