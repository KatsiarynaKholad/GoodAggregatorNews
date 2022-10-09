using GoodAggregatorNews.Core.DataTransferObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoodAggregatorNews.Core.Abstractions
{
    public interface IArticleService
    {
        Task<List<ArticleDto>> GetArticlesByPageNumberAndPageSizeAsync(int pageNumber, int pageSize);

        Task<List<ArticleDto>> GetNewArticlesFromExternalSourcesAsync();
        Task<ArticleDto> GetArticleByIdAsync(Guid id);
        Task<int> CreateArticleAsync(ArticleDto dto);
        Task<int> PatchAsync(Guid modelId, List<PatchModel> patchList);
    }
}
