using GoodAggregatorNews.Core;
using GoodAggregatorNews.Database.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace GoodAggregatorNews.Abstractions.Repositories
{
    public interface IAdditionArticleRepository : IRepository<Article>
    {
       Task UpdateArticleTextAsync (Guid id, string text);
    }
}
