using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoodAggregatorNews.Core.DataTransferObject
{
    public class CommentDto
    {
        public Guid Id { get; set; }
        public string Text { get; set; }
        public DateTime PublicationDate { get; set; }
        public Guid ArticleId { get; set; }
        public Guid ClientId { get; set; }
        public string FullName { get; set; }
    }
}
