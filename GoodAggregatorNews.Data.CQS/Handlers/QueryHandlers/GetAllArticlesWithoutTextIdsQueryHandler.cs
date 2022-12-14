using GoodAggregatorNews.Data.CQS.Queries;
using GoodAggregatorNews.Database;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoodAggregatorNews.Data.CQS.Handlers.QueryHandlers
{
    public class GetAllArticlesWithoutTextIdsQueryHandler : 
        IRequestHandler<GetAllArticlesWithoutTextIdsQuery, Guid[]?>
    {
        private readonly GoodAggregatorNewsContext _context;

        public GetAllArticlesWithoutTextIdsQueryHandler(GoodAggregatorNewsContext context)
        {
            _context = context;
        }

        public async Task<Guid[]?> Handle(GetAllArticlesWithoutTextIdsQuery request,
            CancellationToken token)
        {
            try
            {
                var articlesWithEmptyText = await _context.Articles.AsNoTracking()
                    .Where(art => string.IsNullOrEmpty(art.FullText))
                    .Select(art => art.Id)
                    .ToArrayAsync(token);

                return articlesWithEmptyText;
            }
            catch (Exception ex) 
            {
                Log.Error(ex, "GetAllArticlesWithoutTextIdsQueryHandler was not successful");
                throw;
            }
        }
    }
}
