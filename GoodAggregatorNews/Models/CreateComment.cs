namespace GoodAggregatorNews.Models
{
    public class CreateComment     
    {
        public Guid ArticleId { get; set; }
        public string Text { get; set; }
    }
}
