

namespace BusinessLogic.Models.Accounts
{
    public class AccountResponse
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string role { get; set; }
        public DateTime Created { get; set; }
        public DateTime? IsVerified { get; set; }

    }
}
