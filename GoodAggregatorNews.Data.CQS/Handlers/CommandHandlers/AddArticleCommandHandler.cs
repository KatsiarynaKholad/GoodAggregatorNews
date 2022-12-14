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
    public class AddArticleCommandHandler : 
        IRequestHandler<AddArticleCommand, int> 
    {
        private readonly GoodAggregatorNewsContext _context;
        private readonly IMapper _mapper;

        public AddArticleCommandHandler(GoodAggregatorNewsContext context,
            IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<int> Handle(AddArticleCommand command, CancellationToken token)
        {
            try
            {
                var article = _mapper.Map<Article>(command);
                await _context.Articles.AddAsync(article);
                return await _context.SaveChangesAsync(token);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "AddArticleCommandHandler was not successful");
                throw;
            }
        }
    }
}
