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
    public class AddArticleDataFromRssFeedCommandHandler : 
        IRequestHandler<AddArticleDataFromRssFeedCommand, Unit> 
    {
        private readonly GoodAggregatorNewsContext _context;
        private readonly IMapper _mapper;

        public AddArticleDataFromRssFeedCommandHandler(GoodAggregatorNewsContext context,
            IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Unit> Handle(AddArticleDataFromRssFeedCommand command, CancellationToken token)
        {
            try
            {
                var oldArticleUrl = await _context.Articles
                     .Select(art => art.SourceUrl)
                     .ToArrayAsync(token);

                var entities = command.Articles
                    .Where(dto => !oldArticleUrl.Contains(dto.SourceUrl))
                    .Select(dto => _mapper.Map<Article>(dto)).ToList();

                await _context.Articles.AddRangeAsync(entities);
                await _context.SaveChangesAsync(token);

                return Unit.Value;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "AddAccessTokenCommandHandler was not successful");
                throw;
            }
        }
    }
}
