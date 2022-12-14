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
    public class UpdateArticleRateCommandHandler : 
        IRequestHandler<UpdateArticleRateCommand, Unit> 
    {
        private readonly GoodAggregatorNewsContext _context;

        public UpdateArticleRateCommandHandler(GoodAggregatorNewsContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(UpdateArticleRateCommand command, CancellationToken token)
        {
            try
            {
                var art = await _context.Articles
                    .FirstOrDefaultAsync(art => art.Id.Equals(command.ArticleId), token);

                if (art != null)
                {
                    art.Rate = command.Rate;
                    await _context.SaveChangesAsync(token);
                    return Unit.Value;
                }
                else
                {
                    throw new ArgumentException("Article doesn't exist");
                }

            }
            catch (Exception ex)
            {
                Log.Error(ex, "UpdateArticleRateCommandHandler was not successful");
                throw;
            }
        }
    }
}
