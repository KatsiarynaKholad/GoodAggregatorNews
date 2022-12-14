using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoodAggregatorNews.Data.CQS.Commands
{
    public class AddRefreshTokenCommand : IRequest
    {
        public Guid TokenValue;
        public Guid ClientId;
    }
}
