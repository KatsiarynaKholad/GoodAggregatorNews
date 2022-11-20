using GoodAggregatorNews.Abstractions.Repositories;
using GoodAggregatorNews.Core;
using GoodAggregatorNews.Database;
using GoodAggregatorNews.Database.Entities;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace GoodAggregatorNews.Repositories
{
    public class AdditionArticleRepository : Repository<Article>, IAdditionArticleRepository 
    {
        public AdditionArticleRepository(GoodAggregatorNewsContext context):base(context)
        {

        }
        public async Task UpdateArticleTextAsync(Guid id, string text)
        {
            try
            {
                var article = await DbSet.FirstOrDefaultAsync(art => art.Id.Equals(id));
                if (article != null)
                {
                    article.FullText = text;
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Operation: UpdateArticleTextAsync was not successful");
                throw;
            }
        }
    }
}
