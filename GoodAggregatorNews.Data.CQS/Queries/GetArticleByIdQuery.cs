using GoodAggregatorNews.Database.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoodAggregatorNews.Data.CQS.Queries
{
    public class GetArticleByIdQuery : IRequest<Article?>
    {
        public Guid Id { get; set; }
    }
}
