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
    public class GetArticlesWithEmptyRateIdQueryHandler
        : IRequestHandler<GetArticlesWithEmptyRateIdQuery, List<Guid>>
    {
        private readonly GoodAggregatorNewsContext _context;

        public GetArticlesWithEmptyRateIdQueryHandler(GoodAggregatorNewsContext context)
        {
            _context = context;
        }

        public async Task<List<Guid>> Handle(GetArticlesWithEmptyRateIdQuery request,
            CancellationToken token)
        {
            try
            {
                var artWithoutRate = await _context.Articles.AsNoTracking()
                    .Where(art => art.Rate == null && !string.IsNullOrEmpty(art.FullText))
                    .Select(art => art.Id)
                    .ToListAsync(token);

                return artWithoutRate;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "GetArticlesWithEmptyRateIdQueryHandler was not successful");
                throw;
            }
        }
    }
}
