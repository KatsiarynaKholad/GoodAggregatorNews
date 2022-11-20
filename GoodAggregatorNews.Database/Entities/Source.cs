using GoodAggregatorNews.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoodAggregatorNews.Database.Entities
{
    public class Source : IBaseEntity
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public string? RssUrl { get; set; }
        public SourceType SourceType { get; set; }

        public List<Article> Articles { get; set; }
    }
}
