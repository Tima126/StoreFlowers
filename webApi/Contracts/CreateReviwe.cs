namespace webApi.Contracts
{
    public class CreateReviwe
    {
      
        public int? ProductId { get; set; }

        public int? UserId { get; set; }

        public int Rating { get; set; }

        public string? Comment { get; set; }

        public DateTime? ReviewDate { get; set; }
    }
}
