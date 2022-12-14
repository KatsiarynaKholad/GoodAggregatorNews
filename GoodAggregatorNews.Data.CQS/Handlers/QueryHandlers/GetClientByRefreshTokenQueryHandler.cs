using AutoMapper;
using GoodAggregatorNews.Core.DataTransferObject;
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
    public class GetClientByRefreshTokenQueryHandler : 
        IRequestHandler<GetClientByRefreshTokenQuery, ClientDto?>
    {
        private readonly GoodAggregatorNewsContext _context;
        private readonly IMapper _mapper;


        public GetClientByRefreshTokenQueryHandler(GoodAggregatorNewsContext context,
            IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ClientDto?> Handle(GetClientByRefreshTokenQuery request,
            CancellationToken cancellationtoken)
        {
            try
            {
                var client = (await _context.RefreshTokens.Include(token=>token.Client)
                    .ThenInclude(client=>client.Role)
                    .AsNoTracking()
                    .FirstOrDefaultAsync(token=>token.Token.Equals(request.RefreshToken), 
                    cancellationtoken))?.Client;

                return _mapper.Map<ClientDto>(client);
            }
            catch (Exception ex) 
            {
                Log.Error(ex, "GetClientByRefreshTokenQueryHandler was not successful");
                throw;
            }
        }
    }
}
