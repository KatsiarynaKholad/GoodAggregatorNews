namespace GoodAggregatorNews.Models
{
    public class CreateComment
    {
        public Guid ClientId { get; set; }
        public Guid ArticleId { get; set; }
        public string Text { get; set; }
    }
}
