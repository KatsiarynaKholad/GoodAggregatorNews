using GoodAggregatorNews.Core.Abstractions;
using GoodAggregatorNews.Core.DataTransferObject;
using GoodAggregatorNews.WebAPI.Models.Responses;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace GoodAggregatorNews.WebAPI.Utils
{
    public class JwtUtilSha256 : IJwtUtil
    {
        private readonly IConfiguration _configuration;

        public JwtUtilSha256(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public TokenResponse GenerateToken(ClientDto dto)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8
                .GetBytes(_configuration["Token:JwtToken"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var nowUtc = DateTime.UtcNow;

            var expires = nowUtc.AddMinutes(double
                .Parse(_configuration["Token:ExpiryMinutes"]))
                .ToUniversalTime();

            var claims = new List<Claim>()
            {
                new Claim(JwtRegisteredClaimNames.Sub, dto.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString("D")), 
                new Claim(ClaimTypes.NameIdentifier, dto.Id.ToString("D")),
                new Claim(ClaimTypes.Role, dto.RoleName),
            };


            var jwtToken = new JwtSecurityToken(_configuration["Token:Issuer"],
            _configuration["Token:Issuer"],
            claims,
            expires: expires,
            signingCredentials: credentials);

            var accessToken = new JwtSecurityTokenHandler().WriteToken(jwtToken);

            return new TokenResponse()
            {
                AccessToken = accessToken,
                Role = dto.RoleName,
                TokenExpiration = jwtToken.ValidTo,
                ClientId = dto.Id
            };

        }
    }
}
