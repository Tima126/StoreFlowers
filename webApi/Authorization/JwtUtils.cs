using BusinessLogic.Authorization;
using BusinessLogic.Helpers;
using Domain.Entities;
using Domain.interfaces;
using Domain.Models;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace webApi.Authorization
{
    public class JwtUtils : IJwtUtils
    {
        private readonly IRepositoryWrapper _wrapper;
        private readonly AppSettings _appSettings;

        public JwtUtils(IRepositoryWrapper wrapper, IOptions<AppSettings> appsettings)
        {
            _wrapper = wrapper;
            _appSettings = appsettings.Value;
        }

        public string GenerateJwtToken(User account)
        {
            // генерация токена который будет валидный только 15 минут
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim("id", account.UserId.ToString()) }),
                Expires = DateTime.UtcNow.AddMinutes(15),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }



        public async Task<RefreshToken> GenerateRefreshToken(string ipAddress)
        {
            var refreshToken = new RefreshToken
            {
                Token = Convert.ToHexString(RandomNumberGenerator.GetBytes(64)),
                Expires = DateTime.UtcNow.AddDays(7),
                Created = DateTime.UtcNow,
                CreateByIp = ipAddress
            };

            var tokenIsUniq = (await _wrapper.User.FindByCondition(a => a.RefreshTokens.Any(t => t.Token == refreshToken.Token))).Count == 0;

            if (!tokenIsUniq)
                return await GenerateRefreshToken(ipAddress);

            return refreshToken;




        }

        public int? ValidateJwtToken(string token)
        {
            if (token == null)
                return null;

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);

            try
            {
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,

                    ClockSkew = TimeSpan.Zero



                }, out SecurityToken validatedToken);
                var jwtToken = (JwtSecurityToken)validatedToken;
                var accountId = int.Parse(jwtToken.Claims.First(x => x.Type == "id").Value);
                return accountId;

            }
            catch
            {
                return null;
            }
        }
    }
}
