using GoodAggregatorNews.Core.DataTransferObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoodAggregatorNews.Core.Abstractions
{
    public interface ISourceService
    {
        Task<List<SourceDto>> GetSourceAsync();

        Task<SourceDto> GetSourceByIdAsync(Guid id);
        Task<int> CreateSourceAsync(SourceDto dto);
    }
}
