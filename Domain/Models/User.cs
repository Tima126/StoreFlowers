
using Domain.Entities;




namespace Domain.Models
{


    public class User
    {
        public int UserId { get; set; }

        public string FirstName { get; set; } = null!;

        public string LastName { get; set; } = null!;

        public string Email { get; set; } = null!;

        public string PasswordHash { get; set; } = null!;

        public string PhoneNumber { get; set; } = null;

        public int AddressId { get; set; }

        public virtual Address Address { get; set; }

        public virtual ICollection<Notification> Notifications { get; set; } = new List<Notification>();

        public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

        public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();

        public virtual ICollection<Wishlist> Wishlists { get; set; } = new List<Wishlist>();

        public bool AcceptTerms { get; set; }
        public Role role { get; set; }
        public string? VerificationToken { get; set; }
        public DateTime? Verified { get; set; }
        public bool IsVerified => Verified.HasValue;
        public string? ResetToken { get; set; }
        public DateTime? ResetTokenExpires { get; set; }
        public DateTime? PasswordReset { get; set; }
        public DateTime? Created { get; set; }
        public DateTime? Update { get; set; }
        public List<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();

        public bool OwnsToken(string token)
        {
            return this.RefreshTokens?.Find(x => x.Token == token) != null;
        }
    }
}