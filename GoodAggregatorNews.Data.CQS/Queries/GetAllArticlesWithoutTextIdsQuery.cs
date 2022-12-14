using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoodAggregatorNews.Data.CQS.Queries
{
    public class GetAllArticlesWithoutTextIdsQuery : IRequest<Guid[]?>
    {

    }
}
