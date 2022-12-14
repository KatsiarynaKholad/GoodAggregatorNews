using AutoMapper;
using GoodAggregatorNews.Data.CQS.Commands;
using GoodAggregatorNews.Database;
using GoodAggregatorNews.Database.Entities;
using MediatR;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoodAggregatorNews.Data.CQS.Handlers.CommandHandlers
{
    public class AddRefreshTokenCommandHandler :  IRequestHandler<AddRefreshTokenCommand, Unit> 
    {
        private readonly GoodAggregatorNewsContext _context;
        private readonly IMapper _mapper;

        public AddRefreshTokenCommandHandler(GoodAggregatorNewsContext context,
            IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Unit> Handle(AddRefreshTokenCommand command, CancellationToken token)
        {
            try
            {
                var refreshToken = new RefreshToken()
                {
                    Id = Guid.NewGuid(),
                    Token = command.TokenValue,
                    ClientId = command.ClientId,
                };
    
                await _context.RefreshTokens.AddAsync(refreshToken, token);
                await _context.SaveChangesAsync(token);
    
                return Unit.Value;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "AddRefreshTokenCommandHandler was not successful");
                throw;
            }
        }
    }
}
