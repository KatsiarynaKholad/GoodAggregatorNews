using GoodAggregatorNews.Core.DataTransferObject;
using GoodAggregatorNews.WebAPI.Models.Responses;

namespace GoodAggregatorNews.WebAPI.Utils
{
    public interface IJwtUtil
    {
        TokenResponse GenerateToken(ClientDto dto);
    }
}
