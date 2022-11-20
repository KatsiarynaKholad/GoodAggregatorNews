using GoodAggregatorNews.Core.DataTransferObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoodAggregatorNews.Core.Abstractions
{
    public interface IParseService
    {
        Task AddArticleTextToArticleAsync(Guid articleid);
    }
}
