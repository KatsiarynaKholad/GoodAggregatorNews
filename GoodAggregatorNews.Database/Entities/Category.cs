using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoodAggregatorNews.Database.Entities
{
    public class Category : IBaseEntity
    {
        public Guid Id { get; set; }
        public string CategoryName { get; set; }
        public List<Article> Articles { get; set; }
    }
}
