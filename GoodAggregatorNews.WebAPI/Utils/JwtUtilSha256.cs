using GoodAggregatorNews.Core.Abstractions;
using GoodAggregatorNews.Core.DataTransferObject;
using GoodAggregatorNews.Data.CQS.Commands;
using GoodAggregatorNews.WebAPI.Models.Responses;
using MediatR;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace GoodAggregatorNews.WebAPI.Utils
{
    public class JwtUtilSha256 : IJwtUtil
    {
        private readonly IConfiguration _configuration;
        private readonly IMediator _mediator;


        public JwtUtilSha256(IConfiguration configuration,
            IMediator mediator)
        {
            _configuration = configuration;
            _mediator = mediator;
        }

        public async Task<TokenResponse> GenerateTokenAsync(ClientDto dto)
        {
            try
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

                var refreshToken = Guid.NewGuid();

                await _mediator.Send(new AddRefreshTokenCommand()
                {
                    ClientId = dto.Id,
                    TokenValue = refreshToken,
                });

                return new TokenResponse()
                {
                    AccessToken = accessToken,
                    Role = dto.RoleName,
                    TokenExpiration = jwtToken.ValidTo,
                    ClientId = dto.Id,
                    RefreshToken = refreshToken
                };
            }
            catch (Exception ex)
            {
                Log.Error(ex, "GenerateTokenAsync was not successful");
                throw;
            }

        }

        public async Task RemoveRefreshTokenAsync(Guid requestRefreshToken)
        {
            try
            {
                await _mediator.Send(new RemoveRefreshTokenCommand()
                {
                    TokenValue = requestRefreshToken
                });
            }
            catch (Exception ex)
            {
                Log.Error(ex, "GenerateTokenAsync was not successful");
                throw;
            }
        }
    }
}
