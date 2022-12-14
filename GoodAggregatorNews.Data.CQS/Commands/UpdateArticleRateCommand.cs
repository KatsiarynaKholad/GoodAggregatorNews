using GoodAggregatorNews.Core.DataTransferObject;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoodAggregatorNews.Data.CQS.Commands
{
    public class UpdateArticleRateCommand:IRequest
    {
        public Guid ArticleId { get; set; }
        public double Rate { get; set; }
    }
}
