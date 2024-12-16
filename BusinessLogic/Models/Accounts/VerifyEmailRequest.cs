using System.ComponentModel.DataAnnotations;

namespace BusinessLogic.Models.Accounts
{
    public class VerifyEmailRequest
    {
        [Required]
        public string Token { get; set; }
    }
}