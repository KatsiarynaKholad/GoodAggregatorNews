using GoodAggregatorNews.Core.DataTransferObject;
using GoodAggregatorNews.WebAPI.Models.Responses;

namespace GoodAggregatorNews.WebAPI.Utils
{
    public interface IJwtUtil
    {
        Task<TokenResponse> GenerateTokenAsync(ClientDto dto);
        Task RemoveRefreshTokenAsync(Guid requestRefreshToken);
    }
}
