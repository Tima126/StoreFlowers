using Domain.Entities;
using Domain.Models;

namespace BusinessLogic.Authorization
{
    public interface IJwtUtils
    {
        public string GenerateJwtToken(User account);
        public int? ValidateJwtToken(string token);
        public Task<RefreshToken> GenerateRefreshToken(string ipAddress);
    }
}