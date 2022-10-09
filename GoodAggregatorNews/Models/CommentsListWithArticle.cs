using GoodAggregatorNews.Core.DataTransferObject;

namespace GoodAggregatorNews.Models
{
    public class CommentsListWithArticle
    {
        public Guid ArticleId { get; set; }
        public List<CommentDto> Comments {get;set;}
    }
}
