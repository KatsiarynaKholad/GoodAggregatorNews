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
    public class GetAllSourcesQueryHandler :
       IRequestHandler<GetAllSourcesQuery, List<SourceDto>>
    {
        private readonly GoodAggregatorNewsContext _context;
        private readonly IMapper _mapper;

        public GetAllSourcesQueryHandler(GoodAggregatorNewsContext context,
            IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<SourceDto>> Handle(GetAllSourcesQuery request,
            CancellationToken token)
        {
            try
            {
                var sources = await _context.Sources.AsNoTracking().ToListAsync(token);
                var sourcesDto = _mapper.Map<List<SourceDto>>(sources);
                return sourcesDto;

            }
            catch (Exception ex)
            {
                Log.Error(ex, "GetAllSourcesQueryHandler was not successful");
                throw;
            }
        }
    }
}
