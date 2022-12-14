using GoodAggregatorNews.Core.DataTransferObject;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoodAggregatorNews.Data.CQS.Queries
{
    public class GetClientByRefreshTokenQuery : IRequest<ClientDto?>
    {
        public Guid RefreshToken { get; set; }
    }
}
