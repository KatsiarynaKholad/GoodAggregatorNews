namespace GoodAggregatorNews.WebAPI.Models.Responses
{
    public class TokenResponse
    {
        public string AccessToken {get;set;}
        public string Role { get; set; }
        public Guid ClientId { get; set; }
        public DateTime TokenExpiration {get;set;}
        public Guid RefreshToken { get; internal set; }
    }
}
