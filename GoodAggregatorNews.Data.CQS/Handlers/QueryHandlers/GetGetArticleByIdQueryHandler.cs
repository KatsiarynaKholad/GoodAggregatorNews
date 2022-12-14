using GoodAggregatorNews.Data.CQS.Queries;
using GoodAggregatorNews.Database;
using GoodAggregatorNews.Database.Entities;
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
    public class GetGetArticleByIdQueryHandler :
        IRequestHandler<GetArticleByIdQuery, Article?>
    {
        private readonly GoodAggregatorNewsContext _context;

        public GetGetArticleByIdQueryHandler(GoodAggregatorNewsContext context)
        {
            _context = context;
        }

        public async Task<Article?> Handle(GetArticleByIdQuery request,
            CancellationToken token)
        {
            try
            {
                var entity = await _context.Articles.AsNoTracking()
                    .FirstOrDefaultAsync(art => art.Id.Equals(request.Id), token);
                return entity;

            }
            catch (Exception ex)
            {
                Log.Error(ex, "GetGetArticleByIdQueryHandler was not successful");
                throw;
            }
        }

    }
}
