using System.ComponentModel.DataAnnotations;

namespace BusinessLogic.Models.Accounts
{
    public class ValidateResetTokenRequest
    {
        [Required]
        public string Token { get; set; }
    }
}