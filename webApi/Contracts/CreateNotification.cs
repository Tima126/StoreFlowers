namespace webApi.Contracts
{
    public class CreateNotification
    {
        
        public string Message { get; set; } = null!;
        public DateTime? SentDate { get; set; }
    }
}
