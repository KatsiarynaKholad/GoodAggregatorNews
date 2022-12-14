using AutoMapper;
using GoodAggregatorNews.Data.CQS.Commands;
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

namespace GoodAggregatorNews.Data.CQS.Handlers.CommandHandlers
{
    public class RemoveRefreshTokenCommandHandler :  IRequestHandler<RemoveRefreshTokenCommand, Unit> 
    {
        private readonly GoodAggregatorNewsContext _context;
        private readonly IMapper _mapper;

        public RemoveRefreshTokenCommandHandler(GoodAggregatorNewsContext context,
            IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Unit> Handle(RemoveRefreshTokenCommand command, CancellationToken token)
        {
            try
            {
                var refreshToken = await _context.RefreshTokens
                    .FirstOrDefaultAsync(refreshToken=>command.TokenValue.Equals(refreshToken.Token),
                    token);
               
                _context.RefreshTokens.Remove(refreshToken);
                await _context.SaveChangesAsync(token);

                return Unit.Value;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "RemoveRefreshTokenCommandHandler was not successful");
                throw;
            }
        }
    }
}
